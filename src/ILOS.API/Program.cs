// --- Add these using statements at the top ---
using Microsoft.EntityFrameworkCore;
using ILOS.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// --- Add services to the container ---

// 1. Add services for API controllers
builder.Services.AddControllers();

// 2. Register our ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

// 3. This tells the app to use our API controllers
app.MapControllers();

app.Run();