using AuthManager;
using Blog.Core;
using Blog.Infrastrucure;
using Blog.Persistance;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Service Registry

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddInfrastrucureServices();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog CMS API", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddAuthManager(opt =>
{
    opt.AddAuthentication(typeof(AuthManager.Strategies.AuthenticationStrategies.EntraIdAuthenticationStrategy));
    opt.AddAuthorization(typeof(AuthManager.Strategies.AuthorizationStrategies.RbacAuthorizationStrategy));
});

#endregion

var app = builder.Build();

#region Use Services
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog CMS API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
#endregion

app.Run();