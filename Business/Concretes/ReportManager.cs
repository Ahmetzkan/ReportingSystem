using AutoMapper;
using Business.Abstracts;
using Business.Dtos.Requests.ReportRequests;
using Business.Dtos.Responses.ReportResponses;
using Business.Rules.BusinessRules;
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
    public class ReportManager : IReportService
    {
        IReportDal _ReportDal;
        IMapper _mapper;
        ReportBusinessRules _ReportBusinessRules;

        public ReportManager(IReportDal ReportDal, IMapper mapper, ReportBusinessRules ReportBusinessRules)
        {
            _ReportDal = ReportDal;
            _mapper = mapper;
            _ReportBusinessRules = ReportBusinessRules;
        }

        public async Task<CreatedReportResponse> AddAsync(CreateReportRequest createReportRequest)
        {
            Report Report = _mapper.Map<Report>(createReportRequest);
            Report createdReport = await _ReportDal.AddAsync(Report);
            CreatedReportResponse createdReportResponse = _mapper.Map<CreatedReportResponse>(createdReport);
            return createdReportResponse;
        }

        public async Task<DeletedReportResponse> DeleteAsync(DeleteReportRequest deleteReportRequest)
        {
            await _ReportBusinessRules.IsExistsReport(deleteReportRequest.Id);
            Report Report = await _ReportDal.GetAsync(predicate: r => r.Id == deleteReportRequest.Id);
            Report deletedReport = await _ReportDal.DeleteAsync(Report);
            DeletedReportResponse deletedReportResponse = _mapper.Map<DeletedReportResponse>(deletedReport);
            return deletedReportResponse;
        }

        public async Task<UpdatedReportResponse> UpdateAsync(UpdateReportRequest updateReportRequest)
        {
            await _ReportBusinessRules.IsExistsReport(updateReportRequest.Id);
            Report Report = _mapper.Map<Report>(updateReportRequest);
            Report updatedBlog = await _ReportDal.UpdateAsync(Report);
            UpdatedReportResponse updatedReportResponse = _mapper.Map<UpdatedReportResponse>(updatedBlog);
            return updatedReportResponse;
        }

        public async Task<IPaginate<GetListReportResponse>> GetListAsync(PageRequest pageRequest)
        {
            var Reports = await _ReportDal.GetListAsync(
                index: pageRequest.PageIndex,
                size: pageRequest.PageSize);
            var mappedReports = _mapper.Map<Paginate<GetListReportResponse>>(Reports);
            return mappedReports;
        }
        public async Task<GetListReportResponse> GetByIdAsync(Guid Id)
        {
            var Reports = await _ReportDal.GetAsync(r => r.Id == Id);
            var mappedReports = _mapper.Map<GetListReportResponse>(Reports);
            return mappedReports;
        }

    }
}
