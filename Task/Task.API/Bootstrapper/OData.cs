using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Task.API.Bootstrapper
{
    public static class OData
    {
        public static IEdmModel GetEdmModel<TEntity>() where TEntity : class
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<TEntity>(typeof(TEntity).Name + "s");
            return builder.GetEdmModel();
        }

      
    }
}
