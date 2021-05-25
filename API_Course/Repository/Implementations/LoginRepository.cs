using Mvc.Configurations;
using Mvc.Data.VO;
using Mvc.Repository.Interfaces;
using Mvc.Services.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mvc.Repository.Implementations
{
    public class LoginRepository : ILoginRepository
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginRepository(TokenConfiguration configuration,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            var user = _userRepository.ValidationCredentials(userCredentials);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                };

                var acessToken = _tokenService.GenerateAcessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireTime = DateTime.Now.AddDays(_configuration.DaysToExpire);

                _userRepository.RefreshUserToken(user);

                DateTime createDate = DateTime.Now;
                DateTime expireDate = createDate.AddMinutes(_configuration.Minutes);

                return new TokenVO(
                    true,
                    createDate.ToString(DATE_FORMAT),
                    expireDate.ToString(DATE_FORMAT),
                    acessToken,
                    refreshToken
                    );
            }

            return null;
        }

        public TokenVO ValidateCredentials(TokenVO tokenVO)
        {
            var acessToken = tokenVO.AcessToken;
            var refreshToken = tokenVO.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpireToken(acessToken);
            var userName = principal.Identity.Name;

            var user = _userRepository.ValidationCredentials(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                return null;
            }

            acessToken = _tokenService.GenerateAcessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;

            _userRepository.RefreshUserToken(user);

            DateTime createDate = DateTime.Now;
            DateTime expireDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                true,
                createDate.ToString(DATE_FORMAT),
                expireDate.ToString(DATE_FORMAT),
                acessToken,
                refreshToken
                );
        }

        public bool RevokenToken(string Username)
        {
            return _userRepository.RevokenToken(Username);
        }
    }
}
