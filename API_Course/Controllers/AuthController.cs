using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.Data.VO;
using Mvc.Repository.Interfaces;

namespace Mvc.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;

        public AuthController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn(UserVO userVO)
        {
            if (userVO == null) return BadRequest("Invalid client request.");

            var token = _loginRepository.ValidateCredentials(userVO);

            if (token == null) return Unauthorized();

            return Ok(token);
        }


        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenVO tokenVO)
        {
            if (tokenVO == null) return BadRequest("Invalid client request.");

            var token = _loginRepository.ValidateCredentials(tokenVO);

            if (token == null) return BadRequest("Invalid client request.");

            return Ok(token);
        }
    }
}
