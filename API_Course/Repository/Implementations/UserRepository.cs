using Mvc.Data.VO;
using Mvc.Model;
using Mvc.Repository.Interfaces;
using MVC.Model.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mvc.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlServerContext _context;

        public UserRepository(SqlServerContext context)
        {
            _context = context;
        }

        public User ValidationCredentials(UserVO userVO)
        {
            var pass = ComputeHash(userVO.Password, new SHA256CryptoServiceProvider());

            return _context.users.FirstOrDefault(u => u.UserName.Equals(userVO.UserName) && u.Password.Equals(pass));
        }

        private static string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes);
        }

        public User RefreshUserToken(User user)
        {
            var result = _context.users.SingleOrDefault(p => p.Id.Equals(user.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public User ValidationCredentials(string userName)
        {
            return _context.users.FirstOrDefault(u => u.UserName == userName);
        }

        public bool RevokenToken(string Username)
        {
            var user = _context.users.SingleOrDefault(u => u.UserName.Equals(Username));

            if(user is null)
            {
                return false;
            }

            user.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }
    }
}
