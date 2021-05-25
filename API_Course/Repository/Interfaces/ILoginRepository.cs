using Mvc.Data.VO;

namespace Mvc.Repository.Interfaces
{
    public interface ILoginRepository
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidateCredentials(TokenVO tokenVO);
        bool RevokenToken(string Username);
    }
}
