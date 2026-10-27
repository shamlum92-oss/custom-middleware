using CUSTOMMIDDLEWARE; // <-- Add this at the very top!

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Now Program.cs recognizes your extension method!
app.UseRequestProfiler();

app.MapGet("/api/public/data", () => Results.Ok(new { Message = "Public data accessed!" }));
app.MapGet("/api/secure/data", () => Results.Ok(new { Message = "Confidential payload!" }));

app.Run();