namespace JoyModels.Services.Validation;

public static class GlobalValidation
{
    public static void ValidateRequestUuids(Guid routeUuid, Guid requestUuid)
    {
        if (routeUuid != requestUuid)
            throw new ArgumentException("Route uuid doesn't match with request uuid.");
    }
}