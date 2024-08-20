using System.Text;
using Domain;
using Domain.Database;
using Domain.Database.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ------- Database -------
var connectionString = builder.Configuration.GetValue<string>("Db:ConnectionString");
if (connectionString == null)
{
    throw new InvalidOperationException("Database connection string is null.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString, opt => {});
});

// ------------------------

// Dependency injection
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IAppUow, AppUOW>();

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(
    options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, // TODO: kui see on siin siis äkki controlleris ei pea vaatama kella? UURI!
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

var app = builder.Build();

// TODO: Remove later
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("develop", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyHeader();
    } );
    
});
*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("develop");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();