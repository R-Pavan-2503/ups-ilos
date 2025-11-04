using Supabase;

var builder = WebApplication.CreateBuilder(args);

// --- Add this code ---

// 1. Get the URL and Key from appsettings.json
var supabaseUrl = builder.Configuration["Supabase:Url"]!;
var supabaseKey = builder.Configuration["Supabase:ServiceKey"]!;

// 2. Register the Supabase client as a singleton
builder.Services.AddSingleton<Supabase.Client>(provider =>
    new Supabase.Client(
        supabaseUrl,
        supabaseKey,
        new SupabaseOptions
        {
            AutoConnectRealtime = true // Good to have for later
        }
    )
);
// --- End of code to add ---

builder.Services.AddControllers(); // This line was already there

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
app.MapControllers();
app.Run();