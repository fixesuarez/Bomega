using Bomega.DAL;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Threading.Tasks;

namespace Bomega.WebApp.Authentication
{
    public interface IExternalAuthenticationManager
    {
        Task<string> CreateOrUpdateUser( OAuthCreatingTicketContext context );
        Task CreateOrUpdateUserIndex( string provider, string idApi, string guid );
        Task<User> FindUser( string guid );
    }
}
