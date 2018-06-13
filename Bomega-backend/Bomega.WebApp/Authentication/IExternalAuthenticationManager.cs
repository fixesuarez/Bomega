using Bomega.DAL;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bomega.WebApp.Authentication
{
    public abstract class AuthenticationManager<TUserInfo>
    {
        public async Task OnCreatingTicket( OAuthCreatingTicketContext ctx )
        {
            TUserInfo userInfo = await GetUserInfoFromContext( ctx );
            await CreateOrUpdateUser( userInfo );
            User user = await FindUser( userInfo );
            ctx.Principal = CreatePrincipal( user );
        }

        protected abstract Task<TUserInfo> GetUserInfoFromContext( OAuthCreatingTicketContext ctx );
        protected abstract Task CreateOrUpdateUser( TUserInfo userInfo );
        protected abstract Task CreateOrUpdateUserIndex( string provider, string idApi, string guid );
        protected abstract Task<User> FindUser( TUserInfo userInfo);

        ClaimsPrincipal CreatePrincipal( User user )
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier, user.Guid.ToString(), ClaimValueTypes.String ),
            };
            ClaimsPrincipal principal = new ClaimsPrincipal( new ClaimsIdentity( claims, CookieAuthentication.AuthenticationType, ClaimTypes.Email, string.Empty ) );
            return principal;
        }
    }
}
