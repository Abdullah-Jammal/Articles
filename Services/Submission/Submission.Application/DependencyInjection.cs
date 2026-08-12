namespace Submission.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddValidatorsFromAssemblyContaining<CreateArticleCommandValidator>()
        .AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            config.AddOpenBehavior(typeof(SetUserIdBehavior<,>));
        });
        return services;
    }
}
