using System.Security.Claims;
using System.Text;
using Bomega.WebApp.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Bomega.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            string secretKey = Configuration[ "JwtBearer:SigningKey" ];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey( Encoding.ASCII.GetBytes( secretKey ) );

            services.Configure<TokenProviderOptions>( o =>
            {
                o.Audience = Configuration[ "JwtBearer:Audience" ];
                o.Issuer = Configuration[ "JwtBearer:Issuer" ];
                o.SigningCredentials = new SigningCredentials( signingKey, SecurityAlgorithms.HmacSha256 );
            } );

            services
                .AddAuthentication( CookieAuthentication.AuthenticationScheme )
                .AddCookie( CookieAuthentication.AuthenticationScheme )
                .AddJwtBearer( JwtBearerAuthentication.AuthenticationScheme, o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,

                        ValidateIssuer = true,
                        ValidIssuer = Configuration[ "JwtBearer:Issuer" ],

                        ValidateAudience = true,
                        ValidAudience = Configuration[ "JwtBearer:Audience" ],

                        NameClaimType = ClaimTypes.Email,
                        AuthenticationType = JwtBearerAuthentication.AuthenticationType
                    };
                } )
                .AddFacebook( o =>
                {
                    o.SignInScheme = CookieAuthentication.AuthenticationScheme;
                    o.ClientId = Configuration[ "Authentication:Facebook:ClientId" ];
                    o.ClientSecret = Configuration[ "Authentication:Facebook:ClientSecret" ];
                } )
                .AddSpotify( o =>
                {
                    o.ClientId = Configuration[ "Authentication:Spotify:ClientId" ];
                    o.ClientSecret = Configuration[ "Authentication:Spotify:ClientSecret" ];
                    o.Scope.Add( "playlist-read-private" );
                    o.Scope.Add( "playlist-read-collaborative" );
                    o.SaveTokens = true;
                } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( builder =>
                 builder.WithOrigins( "http://localhost:4200" ) );

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" } );
            } );
        }
    }
}
