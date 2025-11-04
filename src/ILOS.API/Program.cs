using Supabase;
using ILOS.Application.Models;
using ILOS.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ====================================================
// ✅ 1. Configure CORS (for local frontend access)
// ====================================================
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ====================================================
// ✅ 2. Configure Supabase client
// ====================================================
var supabaseUrl = builder.Configuration["Supabase:Url"]!;
var supabaseKey = builder.Configuration["Supabase:ServiceKey"]!;

builder.Services.AddSingleton<Supabase.Client>(provider =>
    new Supabase.Client(
        supabaseUrl,
        supabaseKey,
        new SupabaseOptions
        {
            AutoConnectRealtime = true
        }
    )
);

// ====================================================
// ✅ 3. Register application services
// ====================================================
builder.Services.AddScoped<IRateService, RateService>();

// ====================================================
// ✅ 4. Default ASP.NET service setup
// ====================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ====================================================
// ✅ 5. Build and configure middleware pipeline
// ====================================================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS before controller routing
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
