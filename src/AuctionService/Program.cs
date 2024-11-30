using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//connnect with database service
builder.Services.AddDbContext<AuctionDBContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// add automapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());





var app = builder.Build();


app.UseAuthorization();
app.MapControllers();

// seed the database
try
{
    DbInitializer.InitDB(app);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}


app.Run();

