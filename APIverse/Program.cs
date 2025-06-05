using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "The API", Version = "v1" });
});
builder.Services.AddLogging();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy => policy.WithOrigins("https://localhost:5276") // UI port
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();


// Global error handler (optional)
app.UseExceptionHandler(config =>
{
    config.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        logger.LogError(error, "Unhandled exception occurred.");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { message = "Something went wrong." });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

app.UseCors("AllowUI");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
