using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Task.API.Bootstrapper;
using Task.Data.Models.TaskDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Ioc.RegisterScopes(builder.Services);
OData.ODataRegister(builder.Services);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ODataTutorial", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODataTutorial v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }
//IEdmModel GetEdmModel()
//{
//    var builder = new ODataConventionModelBuilder();
//    builder.EntitySet<Product>("Products");
//    return builder.GetEdmModel();
//}