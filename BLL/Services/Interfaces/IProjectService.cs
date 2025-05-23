using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjects();

        Task<ProjectDetailsDto> GetProjectById(int id);

        Task<int> CreateProject(CreateProjectDto createProjectDto, ApplicationUser User);

        Task<int> UpdateProject(UpdateProjectDto updateProjectDto);
        Task<bool> DeleteProject(int id);
    }
}
