using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBroker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProfileMicroservice.Helpers;
using ProfileMicroservice.MessageHandlers;
using ProfileMicroservice.Repositories;
using ProfileMicroservice.Services;
using ProfileMicroservice.Settings;

namespace ProfileMicroservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region Settings

            // Configure strongly typed settings objects
            var tokenSettingsSection = Configuration.GetSection(nameof(TokenSettings));
            services.Configure<TokenSettings>(tokenSettingsSection);

            var databaseSettingsSection = Configuration.GetSection(nameof(DatabaseSettings));
            services.Configure<DatabaseSettings>(databaseSettingsSection);

            var messageQueueSection = Configuration.GetSection(nameof(MessageQueueSettings));
            services.Configure<MessageQueueSettings>(Configuration.GetSection(nameof(MessageQueueSettings)));

            #endregion

            #region Swagger.io

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Values Api"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            });

            #endregion
            
            services.AddMessageConsumer(
                messageQueueSection.Get<MessageQueueSettings>().Uri,
                "ProfileMicroservice",
                builder => builder.WithHandler<RegisterUserMessageHandler>("RegisterUser")
            );

            #region Dependency Injection

            // Configure DI for database settings
            services.AddSingleton<IDatabaseSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            // Configure DI for application services
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IProfileRepository, ProfileRepository>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();

            #endregion
            
            #region Message Queue
            
            services.AddMessageConsumer(
                messageQueueSection.Get<MessageQueueSettings>().Uri,
                "ProfileMicroservice",
                builder => builder.WithHandler<RegisterUserMessageHandler>("RegisterUser")
            );
            
            #endregion

            #region Authentication

            // Configure JWT authentication
            var tokenSettings = tokenSettingsSection.Get<TokenSettings>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    
                    ValidIssuer = tokenSettings.Issuer,
                    ValidAudience = tokenSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.Secret))
                };
                options.SaveToken = true;
            });

            #endregion
            
            services.AddCors();
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);

            #region Health Checks with dependencies

            services.AddHealthChecks()
                .AddCheck("healthy", () => HealthCheckResult.Healthy())
                .AddMongoDb(
                    databaseSettingsSection.Get<DatabaseSettings>().ConnectionString,
                    tags: new[] {"services"}
                )
                .AddRabbitMQ(
                    new Uri(messageQueueSection.Get<MessageQueueSettings>().Uri),
                    tags: new[] {"services"}
                );

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Swagger.io
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile Microservice");
                });
            }

            // Global CORS policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Routing with authentication and authorization
            // The call to UseAuthorization should appear between app.UseRouting()
            // and app.UseEndpoints(..) for authorization to be correctly evaluated.
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            #region Health Checks

            app.UseHealthChecks("/healthy", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("healthy")
            }); 
            app.UseHealthChecks("/ready", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("services")
            });

            #endregion
        }
    }
}
