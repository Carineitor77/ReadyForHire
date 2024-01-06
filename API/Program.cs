using System.Reflection;
using API.Extensions;
using API.Filters;
using API.Middlewares;
using Domain.Constants;
using Domain.Enums;
using Domain.Options;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

var awsSettings = builder.Configuration.GetSection(nameof(AWSSettings)).Get<AWSSettings>() ?? throw new NullReferenceException(nameof(AWSSettings));

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FluentValidationFilter));
});
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.Load(ApplicationLayers.Application.ToString())));
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.Load(ApplicationLayers.Application.ToString()));
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddConfigurations(awsSettings);
builder.Services.AddCorsPolicy();
builder.Services.AddSwagger();
builder.Services.AddJwtAuth(awsSettings);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(ApiConstants.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();