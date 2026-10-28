using CUSTOMMIDDLEWARE;

var builder = WebApplication.CreateBuilder(args);

// 1. Register API Explorer and Swagger generator services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Enable Swagger middleware in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. Custom middleware extension method
app.UseRequestProfiler();

// 4. Endpoints
app.MapGet("/api/public/data", () => Results.Ok(new { Message = "Public data accessed!" }));
app.MapGet("/api/secure/data", () => Results.Ok(new { Message = "Confidential payload!" }));

app.Run();