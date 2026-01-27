using BootcampCLT.Application.Query;
using BootcampCLT.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, services, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration)
      .ReadFrom.Services(services)
      .Enrich.FromLogContext()
);

// Add services to the container.
builder.Services.AddDbContext<PostgresDbContext>(options =>
options.UseNpgsql(
    builder.Configuration.GetConnectionString("ProductosDb")));

builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(typeof(GetProductoByIdHandler).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // En Producción/Kubernetes sí activamos la redirección
    app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
