using Grpc.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Basket.API.Extensions
{
    internal static class ServerCallIdentityContextExtensions
    {
        public static string? GetUserIdentity(this ServerCallContext context)
        {
            
            var bearer = context.GetHttpContext().Request.Headers["Authorization"].ToString();
            var jwtToken = bearer.Replace("Bearer ", string.Empty);

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHandler.ReadToken(jwtToken) as JwtSecurityToken;
            return jwtSecurityToken.Payload["userId"]?.ToString();
        }
        
        //public static string? GetUserName(this ServerCallContext context)
        //=> context.GetHttpContext().User.FindFirst(
        //    x => x.Type == ClaimTypes.Name)?.Value;
    }
}
