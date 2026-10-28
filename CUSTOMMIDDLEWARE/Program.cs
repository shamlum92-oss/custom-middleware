using CUSTOMMIDDLEWARE.Models;

using CUSTOMMIDDLEWARE.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register SQLite Entity Framework Core DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=blogs.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Example Minimal API endpoints for Blogs matching your schema
app.MapGet("/blogs", async (AppDbContext db) =>
    await db.Blogs.ToListAsync());

app.MapPost("/blogs", async (Blog blog, AppDbContext db) =>
{
    db.Blogs.Add(blog);
    await db.SaveChangesAsync();
    return Results.Created($"/blogs/{blog.Id}", blog);
});

app.MapGet("/blogs/{id}", async (int id, AppDbContext db) =>
    await db.Blogs.FindAsync(id)
        is Blog blog ? Results.Ok(blog) : Results.NotFound());

app.Run();