using Microsoft.EntityFrameworkCore;
using TentHouseRental.BussinessLogic;
using TentHouseRental.DAL;
using TentHouseRental.DAL.Models;
using TentHouseRental.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductDA, ProductDA>();
builder.Services.AddScoped<ICustomerDA, CustomerDA>();
builder.Services.AddScoped<IUserDA, UserDA>();
builder.Services.AddScoped<ITransactionHistoryDA, TransactionHistoryDA>();
builder.Services.AddScoped<ITentHouseRentalService, TentHouseRentalService>();
builder.Services.AddDbContext<TentHouseRentalContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DBCS");
    options.UseSqlServer(connectionString);
});

var LogDirectory = builder.Configuration.GetValue<string>("LogDirectory");
builder.Services.AddSingleton<TentHouseRental.Utils.ILogger>(new Logger(LogDirectory));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500", "https://127.0.0.1:5500")
                   .AllowAnyMethod()  
                   .AllowAnyHeader()  
                   .AllowCredentials();  
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,          
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

app.UseExceptionHandler(
    options =>
    {
        options.Run(
            async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionHandlerPathFeature?.Error != null)
                {
                    var logger = context.RequestServices.GetRequiredService<TentHouseRental.Utils.ILogger>();
                    var exception = exceptionHandlerPathFeature.Error;
                    logger.WriteLog(exception);                  
                }
            }
        );
    }
);

app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
