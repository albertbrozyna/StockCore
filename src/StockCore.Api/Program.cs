using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockCore.Api.Common.Extensions;
using StockCore.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCarter();


var presentationAssembly = typeof(Program).Assembly;
var infrastructureAssembly = typeof(StockCore.Infrastructure.Configuration.InfrastructureServiceInstaller).Assembly;
var applicationAssembly = typeof(StockCore.Application.Configuration.ApplicationServiceInstaller).Assembly;

builder.Services.InstallServicesInAssemblies(
    builder.Configuration,
    presentationAssembly,
    infrastructureAssembly,
    applicationAssembly);


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.UseDatabaseInitializer();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.UseRequestTimeouts();
app.MapCarter();


await app.RunAsync();
