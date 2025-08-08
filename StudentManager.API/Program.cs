using Microsoft.EntityFrameworkCore;
using StudentManager.DAL.Data;

var builder = WebApplication.CreateBuilder(args);
//1.Add DB context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StudentManagerDbContext>(options =>
    options.UseSqlServer(connectionString,b=>b.MigrationsAssembly("StudentManager.API")));
// Add services to the container.
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
app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();
app.Run();
