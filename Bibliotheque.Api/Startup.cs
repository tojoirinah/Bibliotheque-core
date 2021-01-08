
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bibliotheque.Api.Helpers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Bibliotheque.Api
{
    public class Startup
    {
        readonly string ALLOW_ORIGIN = "allowOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // swagger
            ServiceConfigureSwagger(services);
            services.AddSwaggerGenNewtonsoftSupport();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<ApplicationSettings>();
            // JWT
            ServiceConfigureJwt(services, appSettings);

            // CORS
            ServiceConfigureCors(services, appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

          //  app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(ALLOW_ORIGIN);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
          
           // app.UseAuthentication();
           // app.UseMvc();
        }



        #region "SWAGGER"
        void ServiceConfigureSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // resolve Failed to load API definition.
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // Swagger 2.+ support
                var security = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(security);
            });
        }
        #endregion

        #region "JWT"
        void ServiceConfigureJwt(IServiceCollection services, ApplicationSettings settings)
        {
            var key = Encoding.ASCII.GetBytes(settings.JwtSecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        #endregion

        #region "CORS"
        void ServiceConfigureCors(IServiceCollection services, ApplicationSettings settings)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(ALLOW_ORIGIN,
                builder =>
                {
                    builder.WithOrigins(settings.ClientUrl.Split(",").ToArray())
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });
        }
        #endregion
    }
}
