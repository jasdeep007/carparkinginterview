using carparkinginterview.Data;
using carparkinginterview.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddScoped<IParkingService, ParkingService>();

//// Register DbContext with in-memory database
//builder.Services.AddDbContext<CarParkContext>(options =>
//    options.UseInMemoryDatabase("CarParkDb"));

// Register DbContext with SQL Server
builder.Services.AddDbContext<CarParkContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarParkContext>();
    context.SeedData();
}

app.Run();