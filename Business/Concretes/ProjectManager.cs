using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.ProjectRequests;
using Business.Dtos.Responses.ProjectResponses;
using Business.Rules.BusinessRules;
using Core.CrossCuttingConcerns.Transaction;
using Core.DataAccess.Paging;
using DataAccess.Abstracts;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concretes
{
    public class ProjectManager : IProjectService
    {
        IProjectDal _projectDal;
        IMapper _mapper;
        ProjectBusinessRules _projectBusinessRules;

        public ProjectManager(IProjectDal projectDal, IMapper mapper, ProjectBusinessRules projectBusinessRules)
        {
            _projectDal = projectDal;
            _mapper = mapper;
            _projectBusinessRules = projectBusinessRules;
        }

        public async Task<CreatedProjectResponse> AddAsync(CreateProjectRequest createProjectRequest)
        {
            Project project = _mapper.Map<Project>(createProjectRequest);
            Project createdProject = await _projectDal.AddAsync(project);
            CreatedProjectResponse createdProjectResponse = _mapper.Map<CreatedProjectResponse>(createdProject);
            return createdProjectResponse;
        }

        public async Task<DeletedProjectResponse> DeleteAsync(DeleteProjectRequest deleteProjectRequest)
        {
            await _projectBusinessRules.IsExistsProject(deleteProjectRequest.Id);
            Project project = await _projectDal.GetAsync(predicate: p => p.Id == deleteProjectRequest.Id);
            Project deletedProject = await _projectDal.DeleteAsync(project);
            DeletedProjectResponse deletedProjectResponse = _mapper.Map<DeletedProjectResponse>(deletedProject);
            return deletedProjectResponse;
        }

        public async Task<UpdatedProjectResponse> UpdateAsync(UpdateProjectRequest updateProjectRequest)
        {
            await _projectBusinessRules.IsExistsProject(updateProjectRequest.Id);
            Project project = _mapper.Map<Project>(updateProjectRequest);
            Project updatedBlog = await _projectDal.UpdateAsync(project);
            UpdatedProjectResponse updatedProjectResponse = _mapper.Map<UpdatedProjectResponse>(updatedBlog);
            return updatedProjectResponse;
        }

        public async Task<IPaginate<GetListProjectResponse>> GetListAsync(PageRequest pageRequest)
        {
            var projects = await _projectDal.GetListAsync(
                index: pageRequest.PageIndex,
                size: pageRequest.PageSize);
            var mappedProjects = _mapper.Map<Paginate<GetListProjectResponse>>(projects);
            return mappedProjects;
        }
        public async Task<GetListProjectResponse> GetByIdAsync(Guid Id)
        {
            var projects = await _projectDal.GetAsync(p => p.Id == Id);
            var mappedProjects = _mapper.Map<GetListProjectResponse>(projects);
            return mappedProjects;
        }

    }
}
