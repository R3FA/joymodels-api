using System.Transactions;
using AutoMapper;
using JoyModels.Models.DataTransferObjects.CustomRequestTypes;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.src.Database.Entities;
using Microsoft.AspNetCore.Http;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Sso;

public class SsoService : ISsoService
{
    private readonly JoyModelsDbContext _context;
    private readonly IMapper _mapper;

    public SsoService(JoyModelsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SsoReturn> GetByUuid(SsoGet request)
    {
        var pendingUserEntity = await SsoHelperMethods.GetPendingUserEntity(_context, request);
        var pendingUser = _mapper.Map<SsoReturn>(pendingUserEntity);

        return pendingUser;
    }

    public async Task<SsoReturn> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<UserGet> Create(UserCreate user)
    {
        user.ValidateUserCreationArguments();
        await user.ValidateUserCreationDuplicatedFields(_context);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, nameof(UserRoleEnum.Unverified));

        var userEntity = _mapper.Map<User>(user);
        userEntity.SetCustomValuesUserEntity(user, userRoleEntity);

        var pendingUserEntity = _mapper.Map<PendingUser>(userEntity);
        pendingUserEntity.SetCustomValuesPendingUserEntity();

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await userEntity.CreateUser(_context);
            await pendingUserEntity.CreatePendingUser(_context);
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        var updatedUserEntity = _mapper.Map<User>(userEntity, opt => { opt.Items["UserRole"] = userRoleEntity; });
        return _mapper.Map<UserGet>(updatedUserEntity);
    }

    public async Task<UserGet> Verify(SsoVerify request)
    {
        SsoHelperMethods.ValidateOtpCodeValueFormat(request.OtpCode);

        var pendingUserEntity = await SsoHelperMethods
            .GetPendingUserEntity(_context, _mapper.Map<SsoGet>(request));

        SsoHelperMethods.ValidateOtpCodeForUserVerification(_context, pendingUserEntity, request);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, nameof(UserRoleEnum.User));
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await SsoHelperMethods.DeleteAllPendingUserData(_context, request.UserUuid);
            await SsoHelperMethods.UpdateUsersRoleAfterVerification(_context, pendingUserEntity.UserUuid,
                userRoleEntity.Uuid);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        var userEntity = await SsoHelperMethods.GetUserEntity(_context, request.UserUuid);
        var verifiedUser = _mapper.Map<UserGet>(userEntity);

        return verifiedUser;
    }

    public async Task<SuccessReturnDetails> ResendOtpCode(SsoResendOtpCode request)
    {
        await SsoHelperMethods.CheckIfUserIsVerified(_context, request.UserUuid);
        await SsoHelperMethods.DeleteAllPendingUserData(_context, request.UserUuid);

        var pendingUserEntity = _mapper.Map<PendingUser>(request.UserUuid);
        pendingUserEntity.SetCustomValuesPendingUserEntity();
        await pendingUserEntity.CreatePendingUser(_context);

        return new SuccessReturnDetails()
        {
            Type = "Success",
            Title = "Created",
            Detail = "Otp code has been generated and sent to your email.",
            Status = StatusCodes.Status200OK.ToString(),
        };
    }
}