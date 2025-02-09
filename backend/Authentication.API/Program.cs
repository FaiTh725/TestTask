using Authentication.Application.Implmentations;
using Authentication.Application.Interfaces;
using Authentication.Dal;
using Authentication.Dal.Repositories;
using Authentication.Domain.Repositories;
using Authentication.Infastructure.Implementations;
using Authentication.API.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddValidators();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IAuthTokenService, JwtTokenService>();
builder.Services.AddSingleton<IHashService, HashService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
