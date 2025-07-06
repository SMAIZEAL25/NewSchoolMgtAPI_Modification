using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using New_School_Management_API.UploadImage;
using Serilog;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OData;
using New_School_Management_API.Repository.StudentRepository;
using New_School_Management_API.Services.EmailService;
using New_School_Management_API.Services.EmailService.EmailModel;
using New_School_Management_API.Services.StudentServices;
using New_School_Management_API.Domain.MapConfig;
using New_School_Management_API.Domain.Data;
using New_School_Management_API.Domain.Dbcontext;
using New_School_Management_API.Repository.StudentRepository.StudentRepository;



var builder = WebApplication.CreateBuilder(args);

// Create EDM for OData


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore null properties
    })
    .AddOData(opt =>
    {
        opt.AddRouteComponents("api", ODataConfig.GetEdmModel()) // Align with /api/Student route
           .Select().Filter().OrderBy().Count().Expand()
           .SetMaxTop(100).Count(); // Limit query results for performance
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "SchoolManagementAPI", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });
    options.AddSecurityDefinition("CookieAuth", new OpenApiSecurityScheme
    {
        Name = "Cookie",
        In = ParameterLocation.Cookie,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "CookieAuth",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                },
                Scheme = "Bearer", // Corrected: Matches HTTP Authorization header
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        },
        //{
        //    new OpenApiSecurityScheme
        //    {
        //        Reference = new OpenApiReference
        //        {
        //            Type = ReferenceType.SecurityScheme,
        //            Id = "CookieAuth",
        //        },
        //        Scheme = "CookieAuth", // Corrected: Matches scheme name
        //        Name = "CookieAuth",
        //        In = ParameterLocation.Cookie
        //    },
        //    new List<string>()
        //}
    });
});

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc
.WriteTo.Console()
.ReadFrom.Configuration(ctx.Configuration)
.Enrich.WithProperty("Application", "SchoolManagementAPI")
.Filter.ByExcluding(logEvent => logEvent.Properties.ContainsKey("RequestBody") && logEvent.Properties["RequestBody"].ToString().Contains("Password"))); 


// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig).Assembly);

// Register services
builder.Services.AddScoped<IUploadImage, UploadImage>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddMemoryCache();

// EmailService
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Add Authentication (JWT + Cookie)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Use cookies by default
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Challenge with cookies
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Sign in with cookies
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "SchoolManagementAuth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/api/auth/login";
    options.LogoutPath = "/api/auth/logout";
})
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

// Configure Identity with Entity Framework
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<StudentManagementAuthDB>()
    .AddDefaultTokenProviders();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register DbContext
var connectionString = builder.Configuration.GetConnectionString("StudentManagementDB");
builder.Services.AddDbContext<StudentManagementDB>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<StudentManagementAuthDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentManagementAuthDB")));

// Configure CORS with a more secure policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("https://your-frontend.com") // Replace with actual frontend URL
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Required for cookies
    });
});

// Add Rate Limiting (combined configuration)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("BasicRateLimit", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });

    options.AddPolicy("UserBasedRateLimit", context =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirst("uid")?.Value ?? "anonymous"
                : context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: userId => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            }
        );
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.Headers.Add("Retry-After", "60");
        await context.HttpContext.Response.WriteAsync(
            JsonSerializer.Serialize(new { message = "Too many requests. Please try again later." }),
            token);
    };
});

var app = builder.Build();

// Ensure the upload folder exists
var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadImageFloder");
if (!Directory.Exists(uploadFolder))
{
    Directory.CreateDirectory(uploadFolder);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins"); // Use the more secure CORS policy

app.UseHttpsRedirection();

// Add rate limiting middleware before authentication
app.UseRateLimiter();

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