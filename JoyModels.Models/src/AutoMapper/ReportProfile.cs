using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Report;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Report;

namespace JoyModels.Models.AutoMapper;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<Report, ReportResponse>();
        CreateMap<ReportCreateRequest, Report>();
    }
}