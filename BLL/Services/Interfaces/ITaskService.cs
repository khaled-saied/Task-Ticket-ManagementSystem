using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;

namespace BLL.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasks();
        Task<TaskDetailsDto> GetTaskById(int id);
        Task<int> CreateTask(CreateTaskDto createTaskDto);
        Task<int> UpdateTask(UpdateTaskDto updateTaskDto);
        Task<bool> DeleteTask(int id);
        Count GetCount();
    }
}
