using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.TaskDtos;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Classes
{
    public class TaskService(IUnitOfWork _unitOfWork,
                             IMapper _mapper) : ITaskService
    {

        //Get all tasks
        public async Task<IEnumerable<TaskDto>> GetAllTasks()
        {
            var Tasks = await _unitOfWork.GetRepository<TaskK, int>().GetAllActive().ToListAsync();
            var TaskDtos = _mapper.Map<IEnumerable<TaskDto>>(Tasks);
            foreach (var task in TaskDtos)
            {
                UpdateTaskStatus(task);
            }
            return TaskDtos;
        }

        //Get task by id
        public async Task<TaskDetailsDto> GetTaskById(int id)
        {
            var Task = await _unitOfWork.GetRepository<TaskK, int>()
                .GetAllActive()
                .Include(t => t.Project)
                .Include(t => t.Tickets.Where(T => !T.IsDeleted))
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);


            if (Task == null)
                throw new NotFoundException($"Task with id {id} not found");

            var TaskDto = _mapper.Map<TaskDetailsDto>(Task);

            if (TaskDto.Status != TaskStatusEnum.Done.ToString())
            {
                if (DateTime.Now > TaskDto.DueDate)
                    TaskDto.Status = TaskStatusEnum.Blocked.ToString();
            }

            return TaskDto;
        }

        //Create task
        public async Task<int> CreateTask(CreateTaskDto createTaskDto, ApplicationUser user)
        {
            var project = await _unitOfWork.GetRepository<Project, int>().GetByIdAsync(createTaskDto.ProjectId);
            if (project == null)
                throw new NotFoundException($"Project with id {createTaskDto.ProjectId} not found");
            var Task = _mapper.Map<CreateTaskDto, TaskK>(createTaskDto);
            Task.CreatedBy = user.UserName;

            Task.Status = TaskStatusEnum.New;
            var isExist = await _unitOfWork.GetRepository<TaskK, int>().GetAllActive()
                .AnyAsync(t => t.Title == createTaskDto.Title && t.ProjectId == createTaskDto.ProjectId);
            if (isExist)
                throw new ConflictException("Task with the same title already exists in this project.");

            await _unitOfWork.GetRepository<TaskK, int>().AddAsync(Task);
            return await _unitOfWork.SaveChanges();
        }

        //Update task
        public async Task<int> UpdateTask(UpdateTaskDto updateTaskDto)
        {
            var existingTask = await _unitOfWork.GetRepository<TaskK, int>().GetByIdAsync(updateTaskDto.Id);
            if (existingTask == null)
                throw new NotFoundException($"Task with id {updateTaskDto.Id} not found");

            _mapper.Map(updateTaskDto, existingTask); // ✅ mapping to existing tracked entity
            _unitOfWork.GetRepository<TaskK, int>().Update(existingTask); // ✅ optional but fine
            return await _unitOfWork.SaveChanges(); // ✅ actually saves
        }


        //Delete task
        public async Task<bool> DeleteTask(int id)
        {
            var Task = await _unitOfWork.GetRepository<TaskK, int>()
                                        .GetAllActive()
                                        .Include(t => t.Tickets)
                                            .ThenInclude(t => t.Comments)
                                        .FirstOrDefaultAsync(t => t.Id == id);
            if (Task == null)
                throw new NotFoundException($"Task with id {id} not found");

            foreach (var ticket in Task.Tickets)
            {
                foreach (var comment in ticket.Comments)
                {
                    comment.IsDeleted = true; // Soft delete comments
                    _unitOfWork.GetRepository<Comment, int>().Update(comment);
                }
                ticket.IsDeleted = true; // Soft delete tickets
                _unitOfWork.GetRepository<Ticket, int>().Update(ticket);
            }

            Task.IsDeleted = true;
            _unitOfWork.GetRepository<TaskK, int>().Update(Task);
            return await _unitOfWork.SaveChanges() > 0 ? true : false;
        }

        // Update task status based on due date and current time
        private void UpdateTaskStatus(TaskDto task)
        {
            if (task.Status != TaskStatusEnum.Done.ToString())
            {
                if (DateTime.Now > task.DueDate)
                {
                    task.IsOverdue = true;
                }
                else if ((task.DueDate - DateTime.Now).TotalHours <= 24)
                {
                    task.IsDueSoon = true;
                }
            }
        }


        public Count GetCount()
        {
            return _unitOfWork.GetRepository<TaskK, int>().GetCount();
        }

        // Show deleted tasks
        public async Task<IEnumerable<TaskDto>> GetAllDeletedTasks()
        {
            var deletedTasks = await _unitOfWork.GetRepository<TaskK, int>().GetAllDeleted().ToListAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(deletedTasks);

        }
    }
}
