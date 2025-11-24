using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelAvailability;

namespace JoyModels.Models.AutoMapper;

public class ModelAvailabilityProfile : Profile
{
    public ModelAvailabilityProfile()
    {
        CreateMap<ModelAvailabilityResponse, ModelAvailability>();
        CreateMap<ModelAvailability, ModelAvailabilityResponse>();
        CreateMap<ModelAvailabilityCreateRequest, ModelAvailability>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid());
        CreateMap<ModelAvailabilityPatchRequest, ModelAvailabilityResponse>();
    }
}