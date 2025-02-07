using Event.API.Extentions;
using Event.API.Infastructure;
using Event.Dal;
using Event.Dal.Repositories;
using Event.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddBlobStorage(builder.Configuration);

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventMemberRepository, EventmemberRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
