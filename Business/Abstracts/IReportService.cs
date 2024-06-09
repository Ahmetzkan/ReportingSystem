using Business.Dtos.Requests.ReportRequests;
using Business.Dtos.Responses.ReportResponses;
using Core.DataAccess.Paging;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Abstracts
{
    public interface IReportService
    {
        Task<CreatedReportResponse> AddAsync(CreateReportRequest createReportRequest);
        Task<UpdatedReportResponse> UpdateAsync(UpdateReportRequest updateReportRequest);
        Task<DeletedReportResponse> DeleteAsync(DeleteReportRequest deletedProjectRequest);
        Task<IPaginate<GetListReportResponse>> GetListAsync(PageRequest pageRequest);
        Task<GetListReportResponse> GetByIdAsync(Guid Id);
    }
}
