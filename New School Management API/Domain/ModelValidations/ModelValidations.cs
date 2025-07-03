using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace New_School_Management_API.Domain.ModelValidations
{
    public class ValidationModelState : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {

                context.Result = new BadRequestResult();
            }
        }
    }
}
