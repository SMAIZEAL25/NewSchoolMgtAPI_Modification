using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using New_School_Management_API.Data;
using New_School_Management_API.Dbcontext;
using New_School_Management_API.EmailService.EmailModel;
using New_School_Management_API.EmailService;
using New_School_Management_API.MapConfig;
using New_School_Management_API.Repository;
using New_School_Management_API.StudentRepository;
using New_School_Management_API.UploadImage;
using Serilog;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
         options.JsonSerializerOptions.WriteIndented = true;


     });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(Options =>
{
    Options.SwaggerDoc("v1", new() { Title = "SchoolManagementAPI", Version = "v1" });
    Options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });

    Options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                },

                Scheme = "OAuth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            //Array.Empty<string>()
            new List<string> ()
        }
});
});

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig).Assembly);

// Register IUploadImage service
builder.Services.AddScoped<IUploadImage, UploadImage>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddMemoryCache();


//EmailService
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();


// IdentityUser 
builder.Services.Configure<IdentityOptions>(
    options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    });

//Adding Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });

// Ensure correct Auth DB is used // Use APIUser instead of IdentityUser because it's inherit from id
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<StudentManagementAuthDB>()
    .AddDefaultTokenProviders();


//HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register DbContext
var connectionString = builder.Configuration.GetConnectionString("StudentManagementDB");
builder.Services.AddDbContext<StudentManagementDB>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<StudentManagementAuthDB>(options => options.
UseSqlServer(builder.Configuration.GetConnectionString("StudentManagementAuthDB")));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
});


//using OData
//builder.Services.AddControllers().AddOData(Options =>
//{
//    Options.Select().Filter().OrderBy();
//});

var app = builder.Build();

// Ensure the upload folder exists
var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadImageFloder");
if (!Directory.Exists(uploadFolder))
{
    Directory.CreateDirectory(uploadFolder);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// Serve static files from the upload folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadFolder),
    RequestPath = "/UploadImageFloder"
});

app.MapControllers();

app.Run();