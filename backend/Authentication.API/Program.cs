using Authentication.Application.Implmentations;
using Authentication.Application.Interfaces;
using Authentication.Dal;
using Authentication.Dal.Repositories;
using Authentication.Domain.Repositories;
using Authentication.Infastructure.Implementations;
using Authentication.API.Extentions;
using Authentication.Infastructure.BackGroundServices;
using Authentication.API.Middlewares;
using Authentication.Domain.Common;
using Authentication.API.Infastructure;
using Authentication.Dal.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddCorses(builder.Configuration);
builder.Services.AddValidators();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IAuthTokenService, JwtTokenService>();
builder.Services.AddSingleton<IHashService, HashService>();

builder.Services.AddScoped<IDBTransaction, DBTransaction>();
builder.Services.AddScoped<IDBContextFactory, DBContextFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddHostedService<InitializeDBBackgroundService>();
builder.Services.AddHostedService<InitializeRoles>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("Client");

app.UseAuthorization();

app.MapControllers();

app.Run();
