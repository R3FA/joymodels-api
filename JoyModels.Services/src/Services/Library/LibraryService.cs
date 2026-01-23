using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Library;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Library;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Library.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.Library;

public class LibraryService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation)
    : ILibraryService
{
    public async Task<LibraryResponse> GetByUuid(Guid libraryUuid)
    {
        var libraryEntity = await LibraryHelperMethods.GetLibraryEntity(
            context, userAuthValidation, libraryUuid);
        return mapper.Map<LibraryResponse>(libraryEntity);
    }

    public async Task<LibraryResponse> GetByModelUuid(Guid modelUuid)
    {
        var libraryEntity = await LibraryHelperMethods.GetLibraryEntityByModelUuid(
            context, userAuthValidation, modelUuid);
        return mapper.Map<LibraryResponse>(libraryEntity);
    }

    public async Task<PaginationResponse<LibraryResponse>> Search(LibrarySearchRequest request)
    {
        var libraryEntities = await LibraryHelperMethods.SearchLibraryEntities(
            context, userAuthValidation, request);
        return mapper.Map<PaginationResponse<LibraryResponse>>(libraryEntities);
    }

    public async Task<bool> HasUserPurchasedModel(Guid modelUuid)
    {
        return await LibraryHelperMethods.HasUserPurchasedModel(context, userAuthValidation, modelUuid);
    }

    public async Task<string> GetModelDownloadPath(Guid modelUuid)
    {
        var libraryEntity = await LibraryHelperMethods.GetLibraryEntityByModelUuid(
            context, userAuthValidation, modelUuid);
        return libraryEntity.Model.LocationPath;
    }
}