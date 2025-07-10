namespace DotNet8.PosBackendApi.Features.Authentication.Login;

public class DL_Login
{
    private readonly JwtTokenGenerate _tokenGenerator;
    private readonly AppDbContext _context;
    private readonly JwtModel _tokenModel;

    public DL_Login(JwtTokenGenerate tokenGenerator,
        AppDbContext context, IOptionsMonitor<JwtModel> tokenModel)
    {
        _tokenGenerator = tokenGenerator;
        _context = context;
        _tokenModel = tokenModel.CurrentValue;
    }

    public async Task<LoginResponseModel> Login(LoginRequestModel reqModel)
    {
        var responseModel = new LoginResponseModel();
        var item = await _context.TblStaffs.AsNoTracking()
            .FirstOrDefaultAsync(x => x.StaffName == reqModel.UserName);

        if (item is null || item.Password != reqModel.Password.ToHash(_tokenModel.Key))
        {
            responseModel = new LoginResponseModel
            {
                Message = new MessageResponseModel(false, "UserName and Password Incorrect.Please try again."),
            };
            goto result;
        }

        var model = new StaffModel
        {
            StaffId = item.StaffId,
            StaffName = item.StaffName,
            StaffCode = item.StaffCode,
            Position = item.Position,
            MobileNo = item.MobileNo,
        };

        var accessToken = _tokenGenerator.GenerateAccessToken(model);
        responseModel = new LoginResponseModel
        {
            Message = new MessageResponseModel(true, "Login Successful"),
            token = accessToken,
        };
    result:
        return responseModel;
    }
}
