using AutoMapper;
using Business.Dtos.Requests.ReportRequests;
using Business.Dtos.Responses.ReportResponses;
using Core.DataAccess.Paging;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, CreateReportRequest>().ReverseMap();
            CreateMap<Report, CreatedReportResponse>().ReverseMap();

            CreateMap<Report, DeletedReportResponse>().ReverseMap();

            CreateMap<Report, UpdateReportRequest>().ReverseMap();
            CreateMap<Report, UpdatedReportResponse>().ReverseMap();

            CreateMap<Report, GetListReportResponse>().ReverseMap();

            CreateMap<IPaginate<Report>, Paginate<GetListReportResponse>>().ReverseMap();
        }
    }
}
