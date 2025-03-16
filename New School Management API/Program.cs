using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using New_School_Management_API.Dbcontext;
using Serilog;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));


// Connection string for the database
var connectionString = builder.Configuration.GetConnectionString("StudentManagementDB");
builder.Services.AddDbContext<StudentManagementDB>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles( new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UploadImageFloder")),
    RequestPath = "/UploadImageFloder"
});

app.MapControllers();

app.Run();
