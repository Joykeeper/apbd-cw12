using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container
builder.Services.AddControllers();
// Register DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();