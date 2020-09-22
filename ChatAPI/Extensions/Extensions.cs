
using ChatAPI.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Extensions
{
    public static class Extensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Chat API" });
                c.AddSecurityDefinition("Bearer",
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                                Description = "Please enter into field the word 'Bearer' following by space and JWT",
                                Name = "Authorization",
                                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                            });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
                c.ExampleFilters();
            });
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }
        public static string GenerateToken(this User user, string key, string issuer, string audience, int expires = 120)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid, user.Id.ToString())

            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expires),
                signingCredentials: credentials);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;

        }
        public static void ConfigureJwt(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });

        }
        public static int ExtractUserId(this IIdentity identity)
        {
            try
            {
                var claimsIdentity = identity as ClaimsIdentity;
                var sid = claimsIdentity.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid);
                if (sid != null)
                {
                    return Convert.ToInt32(sid.Value);
                }
            }
            catch
            {
                return 0;
            }
            return 0;

        }

    }
}
