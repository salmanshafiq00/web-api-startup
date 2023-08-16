using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using WebApiStartup;
using WebApiStartup.Data;
using WebApiStartup.Interfaces;
using WebApiStartup.Middlewares;
using WebApiStartup.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// logging configuration
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app_log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();
builder.Logging.ClearProviders();

// Add services to the container.

// Add Serilog
builder.Logging.AddSerilog(logger);

// Add ApplicationDbContext with sql server
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Add Automapper
services.AddAutoMapper(typeof(Program));

services.AddControllers();
//services.AddFluentValidationAutoValidation();

services.AddFluentValidation(options =>
{
    // Validate child properties and root collection elements
    options.ImplicitlyValidateChildProperties = true;
    options.ImplicitlyValidateRootCollectionElements = true;

    // Automatic registration of validators in assembly
    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}); 

services.AddScoped<ITokenRepository, TokenRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IProductRepository, ProductRepository>();

// Add Api Versioning
services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
}
);

// Config Api Explorer
services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add Identity
services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("IdentityProvider")
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// password policy
services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Add JWT Bearer Scheme and Config
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        }
    );


services.AddEndpointsApiExplorer();

services.AddSwaggerGen(options =>
{
    //options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo Web API", Version = "v1"});

    //options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    //{
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = JwtBearerDefaults.AuthenticationScheme
    //});

    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = JwtBearerDefaults.AuthenticationScheme
    //            },
    //            Scheme = "Oauth2",
    //            Name = JwtBearerDefaults.AuthenticationScheme,
    //            In = ParameterLocation.Header
    //        },
    //        new List<string>()
    //    }
    //});

});

services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// version description provider
var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseMiddleware<GlobalExceptionhandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
    RequestPath = "/Files"
});

app.MapControllers();

app.Run();
