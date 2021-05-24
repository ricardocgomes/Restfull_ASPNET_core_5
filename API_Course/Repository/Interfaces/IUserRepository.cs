using Mvc.Data.VO;
using Mvc.Model;
using MVC.Repository;

namespace Mvc.Repository.Interfaces
{
    public interface IUserRepository
    {
        User ValidationCredentials(UserVO userVO);
        User ValidationCredentials(string userName);
        User RefreshUserToken(User user);
    }
}
