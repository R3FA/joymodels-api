using System.Transactions;
using AutoMapper;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.src.Database.Entities;
using Microsoft.EntityFrameworkCore;
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
        SsoHelperMethods.ValidateUuidValue(request.PendingUserUuid.ToString());
        SsoHelperMethods.ValidateUuidValue(request.UserUuid.ToString());

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
        user.ValidateUserCreation();

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, nameof(UserRoleEnum.Unverified));

        var userEntity = _mapper.Map<User>(user);
        userEntity.SetCustomValuesUserEntity(user, userRoleEntity);

        var pendingUserEntity = _mapper.Map<PendingUser>(userEntity);
        pendingUserEntity.SetCustomValuesPendingUserEntity();

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            _context.PendingUsers.Add(pendingUserEntity);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        userEntity.UserRoleUu = userRoleEntity;
        return _mapper.Map<UserGet>(userEntity);
    }

    public async Task<UserGet> Verify(SsoVerify request)
    {
        SsoHelperMethods.ValidateUuidValue(request.PendingUserUuid);
        SsoHelperMethods.ValidateUuidValue(request.UserUuid);
        SsoHelperMethods.ValidateOtpCodeValueFormat(request.OtpCode);

        var pendingUserEntity = await SsoHelperMethods
            .GetPendingUserEntity(_context,
                new SsoGet()
                    { PendingUserUuid = request.PendingUserUuid, UserUuid = request.UserUuid });

        await SsoHelperMethods.ValidateOtpCodeForUserVerification(_context, pendingUserEntity, request);

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context, nameof(UserRoleEnum.User));
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.PendingUsers
                .Where(x => x.UserUuid == Guid.Parse(request.UserUuid))
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            await _context.Users
                .Where(x => x.Uuid == Guid.Parse(request.UserUuid))
                .ExecuteUpdateAsync(y => y.SetProperty(z => z.UserRoleUuid,
                    z => userRoleEntity.Uuid));
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        var updatedUserEntity = await SsoHelperMethods.GetUserEntity(_context, request.UserUuid);
        var verifiedUser = _mapper.Map<UserGet>(updatedUserEntity);

        return verifiedUser;
    }

    public async Task Delete(string uuid)
    {
        throw new NotImplementedException();
    }
}