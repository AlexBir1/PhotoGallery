using Microsoft.IdentityModel.Tokens;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhotoGalleryAPI.Services.JWT
{
    public static class TokenMaker
    {
        public static TokenModel CreateToken(Person entity, TokenDescriptorModel descriptor)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, entity.Id.ToString()),
                new Claim(ClaimTypes.Name, entity.Username),
                new Claim(ClaimTypes.Email, entity.Email),
            };

            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(descriptor.Key));
            var credetials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddMinutes(descriptor.ExpiresInMinutes),
                SigningCredentials = credetials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var JWT = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenModel
            {
                Token = tokenHandler.WriteToken(JWT),
                ValidTo = DateTime.UtcNow.AddMinutes(descriptor.ExpiresInMinutes)
            };
        }
    }
}
