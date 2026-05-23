using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BrainHub.Api.Config
{
    public class JwtConfig
    {
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public int ExpirationInMinutes { get; set; } = 120;

        public static void ConfigureJwt(IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt").Get<JwtConfig>();

            if (jwtConfig is null || string.IsNullOrWhiteSpace(jwtConfig.SecretKey))
            {
                throw new InvalidOperationException("Configuracao de JWT nao encontrada.");
            }

            var key = Encoding.UTF8.GetBytes(jwtConfig.SecretKey);

            services.Configure<JwtConfig>(configuration.GetSection("Jwt"));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtConfig.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
