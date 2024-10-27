using DailyNews.Model;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace DailyNews.OData
{
    public class ODataModelBuilder
    {
        public static IEdmModel GetEdmModel() //đại diện cho mô hình EDM (Entity Data Model) trong OData - IEdmModel
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Users>("User"); //định nghĩa một tập hợp thực thể (entity set) cho loại thực thể Users
            odataBuilder.EntitySet<Articles>("Articles");
            odataBuilder.EntitySet<Category>("Category");

            return odataBuilder.GetEdmModel();
        }
    }
}
