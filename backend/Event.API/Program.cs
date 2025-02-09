using Event.API.Extentions;
using Event.API.Infastructure;
using Event.Application.Implementations;
using Event.Application.Interfaces;
using Event.Dal;
using Event.Dal.Repositories;
using Event.Domain.Repositories;
using Event.Infastructure.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddBlobStorage(builder.Configuration);
builder.Services.AddRedisCach(builder.Configuration);
builder.Services.AddValidators();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICachService, CashService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddSingleton<IBlobService, BlobService>();


builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventMemberRepository, EventmemberRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
