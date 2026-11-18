using RepairIT.UsersService.Data;
using RepairIT.UsersService.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UsersDb")));


builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    UsersDbInitializer.Initialiser(context, logger);
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();