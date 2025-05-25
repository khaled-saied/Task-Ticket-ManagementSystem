using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Classes
{
    
    public class ProjectService(IUnitOfWork _unitOfWork,
                                IMapper _mapper,
                                UserManager<ApplicationUser> _userManager) : IProjectService
    {

        //Get all projects
        public async Task<IEnumerable<ProjectDto>> GetAllProjects()
        {
            var projectsQuery = _unitOfWork.GetRepository<Project, int>().GetAllActive();
            var projects = await projectsQuery.ToListAsync(); 
            var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);
            return projectDtos;
        }

        //Get project by id
        public async Task<ProjectDetailsDto> GetProjectById(int id)
        {
            var project = await _unitOfWork.GetRepository<Project, int>()
                                           .GetAllActive()
                                           .Include(p => p.User)
                                           .Include(p => p.Tasks)
                                           .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                throw new NotFoundException($"Project with id {id} not found");
            }
            var projectDto = _mapper.Map<ProjectDetailsDto>(project);
            return projectDto;
        }

        //Create project
        public async Task<int> CreateProject(CreateProjectDto createProjectDto,ApplicationUser User)
        {
            var Project = _mapper.Map<Project>(createProjectDto);
            Project.UserId = User.Id;

            var isExist = _unitOfWork.GetRepository<Project, int>().GetAllActive()
                .Any(p => p.Name == createProjectDto.Name);

            if (isExist)
                throw new ConflictException("Project with the same name already exists.");

            await _unitOfWork.GetRepository<Project, int>().AddAsync(Project);
            return await _unitOfWork.SaveChanges();
        }


        //Update project
        public async Task<int> UpdateProject(UpdateProjectDto updateProjectDto)
        {
            var existingProject = await _unitOfWork.GetRepository<Project, int>().GetByIdAsync(updateProjectDto.Id);

            if (existingProject == null)
                throw new NotFoundException($"Project with id {updateProjectDto.Id} not found");

            _mapper.Map(updateProjectDto, existingProject);

            _unitOfWork.GetRepository<Project, int>().Update(existingProject);
            return await _unitOfWork.SaveChanges();
        }

        //Delete project
        public async Task<bool> DeleteProject(int id)
        {
            var project = await _unitOfWork.GetRepository<Project, int>()
                .GetAllActive()
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.Tickets)
                        .ThenInclude(tc => tc.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                throw new NotFoundException($"Project with id {id} not found");

            // Soft delete all comments
            foreach (var task in project.Tasks)
            {
                foreach (var ticket in task.Tickets)
                {
                    foreach (var comment in ticket.Comments)
                        comment.IsDeleted = true;

                    ticket.IsDeleted = true;
                }

                task.IsDeleted = true;
            }

            project.IsDeleted = true;

            _unitOfWork.GetRepository<Project, int>().Update(project);
            return await _unitOfWork.SaveChanges() > 0;
        }

    }
}
