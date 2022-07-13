using DemoWebshopApi.Data;
using DemoWebshopApi.Data.Entities;
using DemoWebshopApi.Data.Interfaces;
using DemoWebshopApi.Data.Repositories;
using DemoWebshopApi.DTOs;
using DemoWebshopApi.IdentityAuth;
using DemoWebshopApi.Services.Interfaces;
using DemoWebshopApi.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<WebshopContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IIdentityUserManager, IdentityUserManager>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IShoppingBasketService, ShoppingBasketService>();
builder.Services.AddTransient<IShoppingBasketLineService, ShoppingBasketLineService>();

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

    //This is for dev only scenarios when you don’t have a certificate to use.
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
        options.Authority = "https://localhost:7000";
        options.Audience = "https://localhost:7000/resources";
    });

var app = builder.Build();

app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
