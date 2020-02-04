using Alibi.Framework.Business;
using Alibi.Framework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alibi.Framework.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;

        public AuthenticationController(IAuthenticationBusiness authenticationBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserIdentityModel model)
        {
            var user = _authenticationBusiness.Login(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }


        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserIdentityModel model)
        {
            var user = _authenticationBusiness.Register(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}