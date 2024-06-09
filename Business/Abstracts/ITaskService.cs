using Business.Dtos.Requests.TaskRequests;
using Business.Dtos.Responses.TaskResponses;
using Core.DataAccess.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Abstracts
{
    public interface ITaskService
    {
        Task<CreatedTaskResponse> AddAsync(CreateTaskRequest createTaskRequest);
        Task<UpdatedTaskResponse> UpdateAsync(UpdateTaskRequest updateTaskRequest);
        Task<DeletedTaskResponse> DeleteAsync(DeleteTaskRequest deleteTaskRequest);
        Task<IPaginate<GetListTaskResponse>> GetListAsync(PageRequest pageRequest);
        Task<GetListTaskResponse> GetByIdAsync(Guid Id);
    }
}
