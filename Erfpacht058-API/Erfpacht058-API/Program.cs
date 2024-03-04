using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Erfpacht058_API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Voeg Database Context toe
builder.Services.AddDbContext<Erfpacht058_APIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Erfpacht058_APIContext") ?? throw new InvalidOperationException("Connection string 'Erfpacht058_APIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

// Swagger API Documentatie genereren
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Erfpacht058 API",
        Version = "v1",
        Description = "API die als back-end dient voor de Erfpacht058 webapplicatie",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "E. Langendijk",
            Email = "erik.langendijk@leeuwarden.nl",
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Voeg CORS uitzondering toe voor development
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
});

// Voeg oauth authenticatie middels JWT bearer tokens toe
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})   
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
