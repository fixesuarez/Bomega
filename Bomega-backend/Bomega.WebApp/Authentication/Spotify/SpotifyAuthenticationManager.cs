using Bomega.DAL;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bomega.WebApp.Authentication.Spotify
{
    public class SpotifyAuthenticationManager : AuthenticationManager<SpotifyUserInfo>
    {
        readonly UserGateway _userGateway;
        public SpotifyAuthenticationManager(UserGateway userGateway)
        {
            _userGateway = userGateway;
        }

        protected override async Task CreateOrUpdateUser( SpotifyUserInfo userInfo )
        {
            await _userGateway.CreateOrUpdateSpotifyUser(
                userInfo.IdSpotify,
                userInfo.AccessToken );
        }

        protected override Task CreateOrUpdateUserIndex( string provider, string idApi, string guid )
        {
            throw new System.NotImplementedException();
        }

        protected override Task<User> FindUser( SpotifyUserInfo userInfo )
        {
            return _userGateway.FindBySpotifyId( userInfo.IdSpotify );
        }

        protected override async Task<SpotifyUserInfo> GetUserInfoFromContext( OAuthCreatingTicketContext ctx )
        {
            using( var request = new HttpRequestMessage( HttpMethod.Get, ctx.Options.UserInformationEndpoint ) )
            {
                request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
                request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", ctx.AccessToken );

                using( var response = await ctx.Backchannel.SendAsync( request, HttpCompletionOption.ResponseHeadersRead, ctx.HttpContext.RequestAborted ) )
                {
                    response.EnsureSuccessStatusCode();
                    JObject spotifyUser = JObject.Parse( await response.Content.ReadAsStringAsync() );
                    return new SpotifyUserInfo
                    {
                        AccessToken = ctx.AccessToken,
                        IdSpotify = (string) spotifyUser[ "id" ]
                    };
                }
            }
        }
    }

    public class SpotifyUserInfo
    {
        public string IdSpotify { get; set; }
        public string AccessToken { get; set; }
    }
}
