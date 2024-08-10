using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.TaskRequests;
using Business.Dtos.Responses.TaskResponses;
using Business.Rules.BusinessRules;
using Core.DataAccess.Paging;
using DataAccess.Abstracts;
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

        public async Task<DeletedTaskResponse> DeleteAsync(DeleteTaskRequest deleteTaskRequest)
        {
            await _taskBusinessRules.IsExistsTask(deleteTaskRequest.Id);
            Task task = await _taskDal.GetAsync(predicate: t => t.Id == deleteTaskRequest.Id);
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
            var task = await _taskDal.GetAsync(t => t.Id == Id);
            var mappedTask = _mapper.Map<GetListTaskResponse>(task);
            return mappedTask;
        }
    }
}