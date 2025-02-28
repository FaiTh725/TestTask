using Event.API.Extentions;
using Event.API.Infastructure;
using Event.Application.Interfaces;
using Event.Dal;
using Event.Dal.Common;
using Event.Dal.Repositories;
using Event.Domain.Common;
using Event.Domain.Repositories;
using Event.Infastructure.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddBlobStorage(builder.Configuration);
builder.Services.AddRedisCach(builder.Configuration);
builder.Services.AddCorses(builder.Configuration);
builder.Services.AddAutoMapperProfiles();
builder.Services.AddValidators();
builder.Services.AddAuthToSwagger();
builder.Services.ConfigureMediatR();

builder.Services.AddScoped<ICachService, CashService>();
builder.Services.AddSingleton<IBlobService, BlobService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventMemberRepository, EventmemberRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHostedService<InitializeDBBackgroundService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Client");

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
