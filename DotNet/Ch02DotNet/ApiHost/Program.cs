using ApiHost;
using ApiHost.DbContexts;
using ApiSharedLib.Services;
using ApiSharedLib.VideoRequests;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVideoServiceRepository, VideoServiceRepository>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<VideoContext>(options =>
{
    options.UseSqlite(@"Data Source=library.db");
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies([
            typeof(VideoRequest).Assembly
            ]);
    cfg.Lifetime = ServiceLifetime.Transient;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.ResetDatabaseAsync();

app.Run();
