using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Classes
{
    class TaskService(IUnitOfWork _unitOfWork,
                             IMapper _mapper) : ITaskService
    {

        //Get all tasks
        public async Task<IEnumerable<TaskDto>> GetAllTasks()
        {
            var Tasks = await _unitOfWork.GetRepository<TaskK,int>().GetAllActive().ToListAsync();
            var TaskDtos = _mapper.Map<IEnumerable<TaskDto>>(Tasks);
            return TaskDtos;
        }

        //Get task by id
        public async Task<TaskDetailsDto> GetTaskById(int id)
        {
            var Task = await _unitOfWork.GetRepository<TaskK, int>().GetByIdAsync(id);
            if (Task == null)
                throw new NotFoundException($"Task with id {id} not found");

            var TaskDto = _mapper.Map<TaskDetailsDto>(Task);
            return TaskDto;
        }

        //Create task
        public async Task<int> CreateTask(CreateTaskDto createTaskDto)
        {
            var project = await _unitOfWork.GetRepository<Project, int>().GetByIdAsync(createTaskDto.ProjectId);
            if (project == null)
                throw new NotFoundException($"Project with id {createTaskDto.ProjectId} not found");
            var Task= _mapper.Map<CreateTaskDto, TaskK>(createTaskDto);

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

            var Task = _mapper.Map<TaskK>(updateTaskDto);
            _unitOfWork.GetRepository<TaskK, int>().Update(Task);
            return await _unitOfWork.SaveChanges();
        }

        //Delete task
        public async Task<bool> DeleteTask(int id)
        {
            var Task = await _unitOfWork.GetRepository<TaskK, int>().GetByIdAsync(id);
            if (Task == null)
                throw new NotFoundException($"Task with id {id} not found");
            Task.IsDeleted = true;
            _unitOfWork.GetRepository<TaskK, int>().Update(Task);
            return await _unitOfWork.SaveChanges() > 0 ? true : false;
        }
    }
}
