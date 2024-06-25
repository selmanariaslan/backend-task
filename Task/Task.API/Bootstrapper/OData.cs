using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Task.Core.Entities;
using Task.Data.Models.TaskDb;

namespace Task.API.Bootstrapper
{
    public static class OData
    {
        public static void ODataRegister(IServiceCollection services)
        {
            services.AddControllers().AddOData(opt => opt.AddRouteComponents("v1", OData.GetEdmModel<Product>())
            .Select()
            .Filter()
            .OrderBy()
            .Expand()
            .SetMaxTop(100)
            .Count());
        }

        public static IEdmModel GetEdmModel<TEntity>() where TEntity : class
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<TEntity>(typeof(TEntity).Name + "s");
            return builder.GetEdmModel();
        }


    }
}
