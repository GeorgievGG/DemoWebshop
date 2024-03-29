using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Data.Interfaces;
using DemoWebshopApi.Data.Repositories;
using DemoWebshopApi.DTOs;
using DemoWebshopApi.IdentityAuth;
using DemoWebshopApi.Middleware;
using DemoWebshopApi.Services.Interfaces;
using DemoWebshopApi.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlinePayments.Sdk;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.Configure<OgoneSettings>(builder.Configuration.GetSection("OgoneSettings"));
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header (NO NEED for the word Bearer). Example: \"Authorization: {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, //SecuritySchemeType.ApiKey
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },

                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var corsSource = builder.Configuration.GetSection("CorsSettings")["AllowedCorsSource"];
builder.Services.AddCors(p => p.AddPolicy(builder.Configuration.GetSection("CorsSettings")["PolicyName"], builder =>
{
    builder.WithOrigins(corsSource).AllowAnyMethod().AllowAnyHeader();
}));

var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<WebshopContext>(options => 
{ 
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IIdentityUserManager, IdentityUserManager>();
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IShoppingBasketService, ShoppingBasketService>();
builder.Services.AddTransient<IShoppingBasketLineService, ShoppingBasketLineService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

var paymentPlatformConfig = builder.Configuration.GetSection("OgoneSettings");
builder.Services.AddScoped<IClient>(_ => Factory.CreateClient
(
    paymentPlatformConfig["ApiKey"],
    paymentPlatformConfig["ApiSecret"],
    new Uri(paymentPlatformConfig["ApiEndpoint"]),
    paymentPlatformConfig["Integrator"])
);

builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<WebshopContext>();

builder.Services.AddIdentityServer((options) =>
{
    options.EmitStaticAudienceClaim = true;
})

    //This is for dev only scenarios when you don�t have a certificate to use.
    .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
    .AddInMemoryClients(IdentityConfig.Clients)
    .AddDeveloperSigningCredential()
    .AddResourceOwnerValidator<PasswordValidator>();

// omitted for brevity
// Authentication
// Adds the asp.net auth services
builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy =>
                    policy.RequireRole("Admin"));

        options.AddPolicy("User", policy =>
                    policy.RequireRole("User"));
    })

    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    // Adds the JWT bearer token services that will authenticate each request based on the token in the Authorize header
    // and configures them to validate the token with the options

    .AddJwtBearer(options =>
    {
        options.Authority = "https://demo-webshop-webapp.azurewebsites.net";
        options.Audience = "https://demo-webshop-webapp.azurewebsites.net/resources";
    });

var app = builder.Build();

await DatabaseSeeder.Seed(app.Services);
app.UseCors(builder.Configuration.GetSection("CorsSettings")["PolicyName"]);
app.UseIdentityServer();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalErrorHandler>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
