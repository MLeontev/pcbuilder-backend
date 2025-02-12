using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pcbuilder.Api.Validators.Users;
using pcbuilder.Application.Interfaces;
using pcbuilder.Application.Services.BuildService;
using pcbuilder.Application.Services.CaseService;
using pcbuilder.Application.Services.CoolerService;
using pcbuilder.Application.Services.CpuService;
using pcbuilder.Application.Services.GpuService;
using pcbuilder.Application.Services.MotherboardService;
using pcbuilder.Application.Services.PowerSupplyService;
using pcbuilder.Application.Services.RamService;
using pcbuilder.Application.Services.StorageService;
using pcbuilder.Application.Services.UserService;
using pcbuilder.Domain.Interfaces;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Services;
using pcbuilder.Infrastructure.Authentication;
using pcbuilder.Infrastructure.Persistence;
using pcbuilder.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions))
    .Get<JwtOptions>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICpuService, CpuService>();
builder.Services.AddScoped<ICpuRepository, CpuRepository>();

builder.Services.AddScoped<IMotherboardService, MotherboardService>();
builder.Services.AddScoped<IMotherboardRepository, MotherboardRepository>();

builder.Services.AddScoped<IRamService, RamService>();
builder.Services.AddScoped<IRamRepository, RamRepository>();

builder.Services.AddScoped<ICoolerService, CoolerService>();
builder.Services.AddScoped<ICoolerRepository, CoolerRepository>();

builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IStorageRepository, StorageRepository>();

builder.Services.AddScoped<IGpuService, GpuService>();
builder.Services.AddScoped<IGpuRepository, GpuRepository>();

builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<ICaseRepository, CaseRepository>();

builder.Services.AddScoped<IPowerSupplyService, PowerSupplyService>();
builder.Services.AddScoped<IPowerSupplyRepository, PowerSupplyRepository>();

builder.Services.AddScoped<IBuildService, BuildService>();
builder.Services.AddScoped<IBuildRepository, BuildRepository>();
builder.Services.AddScoped<CompatibilityChecker>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();