global using Postieri.Interfaces;
global using Postieri.DTOs;
using Postieri.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Postieri.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MimeKit;
using NuGet.Common;
using Swashbuckle.AspNetCore.Filters;
using FluentValidation;
using Postieri;
using Postieri.Validators;
using Postieri.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Web.Http.Validation.Providers;
using Postieri.Models;
using System.Text.Json.Serialization;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using AutoMapper;
using Postieri.Mappings;
using Postieri.Services.EmailService;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("https://postieri.herokuapp.com",
                                              "http://localhost:3000").AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});



builder.Services.AddControllers();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddFluentValidation(fv =>
{
   
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
    NewMethod(fv);
});

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add<ValidationFilter>();
//});
builder.Services.Configure<ApiBehaviorOptions>(options =>
 {
     options.SuppressModelStateInvalidFilter = true;
 });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<IExportDataExcelService, ExportDataExcelService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExportDataPdfService, ExportDataPdfService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDeliveryPriceService, DeliveryPriceService>();
builder.Services.AddScoped<IBusinessIntegrationService, BusinessIntegrationService>();
builder.Services.AddScoped<IDashboard, DashboardService>();
builder.Services.AddAutoMapper(typeof(OrderMapper).Assembly);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICourierService, CourierService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddSingleton<WebSocketServerConnectionManager>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseWebSockets();

app.Run();


void NewMethod(FluentValidationMvcConfiguration fv)
{
    fv.ImplicitlyValidateChildProperties = true;
}

