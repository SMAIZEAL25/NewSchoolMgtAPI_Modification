
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using New_School_Management_API.Entities;

namespace New_School_Management_API.MapConfig
{
    public class ODataConfig
    {
        public static IEdmModel GetEdmModel() // Make this public
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<StudentRecord>("Students");
            odataBuilder.EntitySet<CourseRegistration>("CourseRegistration");
            odataBuilder.EntitySet<Course>("Courses");
            return odataBuilder.GetEdmModel();
        }
    }
}
