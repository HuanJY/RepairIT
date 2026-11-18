using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.CatalogService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CatalogDb")));

builder.Services.AddScoped<IOrdinateurService,OrdinateurService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    CatalogDbInitializer.Initialiser(context, logger);
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();