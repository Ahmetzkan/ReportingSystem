using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.TaskRequests;
using Business.Dtos.Requests.TaskRequests;
using Business.Dtos.Responses.TaskResponses;
using Business.Dtos.Responses.TaskResponses;
using Business.Rules.BusinessRules;
using Core.DataAccess.Paging;
using DataAccess.Abstracts;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Entities.Concretes.Task;

namespace Business.Concretes
{
    public class TaskManager : ITaskService
    {
        ITaskDal _taskDal;
        IMapper _mapper;
        TaskBusinessRules _taskBusinessRules;

        public TaskManager(ITaskDal taskDal, IMapper mapper, TaskBusinessRules taskBusinessRules)
        {
            _taskDal = taskDal;
            _mapper = mapper;
            _taskBusinessRules = taskBusinessRules;
        }

        public async Task<CreatedTaskResponse> AddAsync(CreateTaskRequest createTaskRequest)
        {
            Task task = _mapper.Map<Task>(createTaskRequest);
            Task createdTask = await _taskDal.AddAsync(task);
            CreatedTaskResponse createdTaskResponse = _mapper.Map<CreatedTaskResponse>(createdTask);
            return createdTaskResponse;
        }
        public async Task<DeletedTaskResponse> DeleteAsync(Guid id)
        {
            await _taskBusinessRules.IsExistsTask(id);
            Task task = await _taskDal.GetAsync(predicate: l => l.Id == id);
            Task deletedTask = await _taskDal.DeleteAsync(task);
            DeletedTaskResponse deletedTaskResponse = _mapper.Map<DeletedTaskResponse>(deletedTask);
            return deletedTaskResponse;
        }

        public async Task<UpdatedTaskResponse> UpdateAsync(UpdateTaskRequest updateTaskRequest)
        {
            await _taskBusinessRules.IsExistsTask(updateTaskRequest.Id);
            Task task = _mapper.Map<Task>(updateTaskRequest);
            Task updatedBlog = await _taskDal.UpdateAsync(task);
            UpdatedTaskResponse updatedTaskResponse = _mapper.Map<UpdatedTaskResponse>(updatedBlog);
            return updatedTaskResponse;
        }

        public async Task<IPaginate<GetListTaskResponse>> GetListAsync(PageRequest pageRequest)
        {
            var tasks = await _taskDal.GetListAsync(
                index: pageRequest.PageIndex,
                size: pageRequest.PageSize);
            var mappedTasks = _mapper.Map<Paginate<GetListTaskResponse>>(tasks);
            return mappedTasks;
        }
        public async Task<GetListTaskResponse> GetByIdAsync(Guid Id)
        {
            var tasks = await _taskDal.GetAsync(p => p.Id == Id);
            var mappedTasks = _mapper.Map<GetListTaskResponse>(tasks);
            return mappedTasks;
        }
    }
}