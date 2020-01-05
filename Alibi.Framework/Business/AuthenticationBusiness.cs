using Alibi.Framework.DbContext;
using Alibi.Framework.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Alibi.Framework.Business
{
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        private readonly IRepository<UserIdentityModel> _userRepository;
        private readonly AppSettings _appSettings;

        public AuthenticationBusiness(IRepository<UserIdentityModel> userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public UserIdentityModel Login(string username, string password)
        {
            var res = _userRepository.GetUsers<UserIdentityModel>();
            var user = _userRepository.FindBy(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public UserIdentityModel Register(UserIdentityModel model)
        {
            _userRepository.Save(model);
            _userRepository.Dispose();

            return model;
        }
    }
}