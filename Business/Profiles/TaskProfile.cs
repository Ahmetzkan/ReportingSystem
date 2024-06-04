using AutoMapper;
using Business.Dtos.Requests.TaskRequests;
using Business.Dtos.Responses.TaskResponses;
using Core.DataAccess.Paging;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = Entities.Concretes.Task;

namespace Business.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, CreateTaskRequest>().ReverseMap();
            CreateMap<Task, CreatedTaskResponse>().ReverseMap();

            CreateMap<Task, DeletedTaskResponse>().ReverseMap();

            CreateMap<Task, UpdateTaskRequest>().ReverseMap();
            CreateMap<Task, UpdatedTaskResponse>().ReverseMap();

            CreateMap<Task, GetListTaskResponse>().ReverseMap();

            CreateMap<IPaginate<Task>, Paginate<GetListTaskResponse>>().ReverseMap();
        }
    }
}
