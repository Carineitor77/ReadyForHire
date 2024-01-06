using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Filters;

public class FluentValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            var isBindingSourceBody = parameter.BindingInfo?.BindingSource == BindingSource.Body;
            var isBindingSourceQuery = parameter.BindingInfo?.BindingSource == BindingSource.Query;
            var isParameterClass = parameter.ParameterType.IsClass;
            
            if (isBindingSourceBody || (isBindingSourceQuery && isParameterClass))
            {
                var validatorObj = _serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(parameter.ParameterType));
                
                if (validatorObj is IValidator validator)
                {
                    var subject = context.ActionArguments[parameter.Name];
                    
                    if (subject is not null)
                    {
                        var result = await validator.ValidateAsync(new ValidationContext<object>(subject), context.HttpContext.RequestAborted);
                        
                        if (!result.IsValid)
                        {
                            var error = result.Errors.FirstOrDefault()?.ErrorMessage;
                            
                            context.Result = new BadRequestObjectResult(error);
                            return;
                        }
                    }
                }
            }
        }
        
        await next();
    }
}