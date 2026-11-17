using RepairIT.UsersService.Data;
using RepairIT.UsersService.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlite("Data Source=catalog.db"));


builder.Services.AddScoped<UtilisateurService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();