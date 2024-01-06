using System.Reflection;
using FluentValidation;

namespace API.Extensions;

public static class FluentValidationExtensions
{
    public static IServiceCollection AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var validatorType = typeof(IValidator<>);
        
        var validatorTypes = assembly.GetTypes()
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType));

        foreach (var validator in validatorTypes)
        {
            var dtoType = validator.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType)
                .Select(i => i.GenericTypeArguments[0])
                .FirstOrDefault();

            if (dtoType is not null)
            {
                services.AddScoped(validatorType.MakeGenericType(dtoType), validator);
            }
        }

        return services;
    }
}
