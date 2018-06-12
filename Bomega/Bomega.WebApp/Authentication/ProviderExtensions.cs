using Bomega.WebApp.Authentication.Spotify;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bomega.WebApp.Authentication
{
    public static class ProviderExtensions
    {
        public static AuthenticationBuilder AddSpotify( this AuthenticationBuilder builder )
            => builder.AddSpotify( SpotifyDefaults.AuthenticationScheme, _ => { } );

        public static AuthenticationBuilder AddSpotify( this AuthenticationBuilder builder, Action<SpotifyOptions> configureOptions )
            => builder.AddSpotify( SpotifyDefaults.AuthenticationScheme, configureOptions );

        public static AuthenticationBuilder AddSpotify( this AuthenticationBuilder builder, string authenticationScheme, Action<SpotifyOptions> configureOptions )
            => builder.AddSpotify( authenticationScheme, SpotifyDefaults.DisplayName, configureOptions );

        public static AuthenticationBuilder AddSpotify( this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<SpotifyOptions> configureOptions )
            => builder.AddOAuth<SpotifyOptions, SpotifyHandler>( authenticationScheme, displayName, configureOptions );
    }
}
