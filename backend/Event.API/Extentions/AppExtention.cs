using Application.Shared.Exceptions;
using Azure.Storage.Blobs;
using Event.API.Configuraions;
using Event.API.Contracts.Event;
using Event.API.Validators.Event;
using Event.API.Validators.Member;
using Event.Application;
using Event.Application.Behaviors;
using Event.Application.Command.Event.UpdateEvent;
using Event.Application.Command.EventMember.PaticipateMember;
using Event.Application.Mappings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

namespace Event.API.Extentions
{
    public static class AppExtention
    {
        public static void AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var blobConf = configuration.GetSection("BlobStorage")
                .Get<BlobStorage>()
                ?? throw new AplicationConfigurationException("Blob Storage");

            var connectionString = $"DefaultEndpointsProtocol=http;AccountName={blobConf.AccountName};" +
                $"AccountKey={blobConf.Key};" +
                $"BlobEndpoint={blobConf.BaseUrl}:{blobConf.Port}/{blobConf.AccountName};";

            services.AddSingleton(_ => new BlobServiceClient(connectionString));
        }

        public static void AddRedisCach(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = configuration.GetConnectionString("RedisConnection") ??
                throw new AplicationConfigurationException("Redis Connection String");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "Events";
            });

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnection));
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CreateEventRequest>, CreateEventValidator>();
            services.AddScoped<IValidator<UpdateEventCommand>, UpdateEventValidator>();
            services.AddScoped<IValidator<PaticipateMemberCommand>, CreateMemberValidator>();
        }

        public static void AddAutoMapperProfiles(this IServiceCollection service)
        {
            service.AddAutoMapper(conf =>
                {
                    conf.AddProfile<EventMemberProfile>();
                    conf.AddProfile<EventProfile>();
                });
        }

        // Implement Proxy(Yarp or using Nginx)
        public static void AddCorses(this IServiceCollection service, IConfiguration configuration)
        {
            var clientUrlHttp = configuration
                .GetValue<string>("ApiList:ClientHttp")
                ?? throw new ArgumentException("Client Url Is Null");

            var clientUrlHttps = configuration
                .GetValue<string>("ApiList:ClientHttps")
                ?? throw new ArgumentException("Client Url Is Null");

            service.AddCors(conf => conf.AddPolicy("Client", policy =>
            {
                policy.WithOrigins(clientUrlHttp, clientUrlHttps);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            }));
        }

        public static void AddJwtService(this IServiceCollection service, IConfiguration configuration)
        {
            var jwtConf = configuration.GetSection("JwtSetting").Get<JwtToken>() ??
                throw new AplicationConfigurationException("Jwt Token") ;

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = jwtConf.Audience,
                        ValidIssuer = jwtConf.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(jwtConf.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var token = ctx.Request.Cookies["token"];
                            if(!string.IsNullOrEmpty(token))
                            {
                                ctx.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            service.AddAuthorization();
        }

        public static void AddAuthToSwagger(this IServiceCollection service)
        {
            service.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Event API",
                    Version = "v1",
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "Jwt Auth",
                    In = ParameterLocation.Header | ParameterLocation.Cookie,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Input Jwt Token",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }

        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AppAssemblyReference).Assembly);
                cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
            });

        }
    }
}
