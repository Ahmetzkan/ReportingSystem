using Business.Dtos.Requests.AuthRequests;
using Business.Messages;
using Core.Business.Rules;
using DataAccess.Abstracts;
using Kps;

namespace Business.Rules.BusinessRules;

public class UserBusinessRules : BaseBusinessRules
{
    IUserDal _userDal;
    private readonly KPSPublicSoapClient _kPSPublicSoapClient;

    public UserBusinessRules(IUserDal userDal, KPSPublicSoapClient kPSPublicSoapClient)
    {
        _userDal = userDal;
        _kPSPublicSoapClient = kPSPublicSoapClient;
    }

    public async Task IsExistsUser(Guid userId)
    {
        var result = await _userDal.GetAsync(
            predicate: a => a.Id == userId,
            enableTracking: false);

        if (result == null)
        {
            throw new BusinessException(BusinessMessages.DataNotFound);
        }
    }

    public async Task VerifyTcKimlikNo(RegisterAuthRequest registerAuthRequest)
    {
        long.TryParse(registerAuthRequest.TcNo, out long Tc);
        TCKimlikNoDogrulaResponse tcKimlikNoDogrulaResponse = await _kPSPublicSoapClient.TCKimlikNoDogrulaAsync(
        Tc, registerAuthRequest.FirstName, registerAuthRequest.LastName, registerAuthRequest.BirthDate.Year);

        if (!tcKimlikNoDogrulaResponse.Body.TCKimlikNoDogrulaResult)
        {
            throw new BusinessException(BusinessMessages.TcNumberVerifiy);
        }
    }

    public async Task IsExistsUserByMail(string email)
    {
        var result = await _userDal.GetAsync(
            predicate: a => a.Email == email,
            enableTracking: false);

        if (result == null)
        {
            throw new BusinessException(BusinessMessages.DataNotFound);
        }
    }

    public async Task IsExistsUserMail(string email)
    {
        var result = await _userDal.GetAsync(
            predicate: a => a.Email == email,
            enableTracking: false);

        if (result != null)
        {
            throw new BusinessException(BusinessMessages.DataAvailable);
        }
    }

    public async Task IsExistsResetToken(string resetToken)
    {
        var test = await _userDal.GetListAsync();

        var result = await _userDal.GetAsync(
            predicate: a => a.PasswordReset == resetToken,
            enableTracking: false);

        if (result == null)
        {
            throw new BusinessException(BusinessMessages.DataNotFound);
        }
    }
}