using System.Transactions;
using AutoMapper;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.src.Database.Entities;

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

    public async Task<SsoGet> GetByUuid(string uuid)
    {
        SsoHelperMethods.ValidateUuidValue(uuid);

        var pendingUserEntity = await SsoHelperMethods.GetPendingUserEntity(_context, uuid);
        var pendingUser = _mapper.Map<SsoGet>(pendingUserEntity);

        return pendingUser;
    }

    public async Task<SsoGet> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<UserGet> Create(UserCreate user)
    {
        user.ValidateUserCreation();

        var userRoleEntity = await SsoHelperMethods.GetUserRoleEntity(_context);

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
            throw new TransactionException(ex.Message, ex);
        }

        userEntity.UserRoleUu = userRoleEntity;
        return _mapper.Map<UserGet>(userEntity);
    }

    public async Task<UserGet> Verify()
    {
        throw new NotImplementedException();
    }

    public async Task Delete(string uuid)
    {
        throw new NotImplementedException();
    }
}