using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocksMessaging.MassTransit;
public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            //sets up the naming convention for the endpoints used by MassTransit.Kebab case is a naming convention where words are lowercase and separated by hyphens (e.g., order-queue, user-updated-event)
            //In MassTransit, an endpoint is a point where messages are sent or received. Endpoints typically represent queues or topics in the message broker (e.g., RabbitMQ).
            // When you use SetKebabCaseEndpointNameFormatter, MassTransit will automatically convert the names of your endpoints to kebab case. For example:
            //If you have a consumer named OrderCreatedConsumer, the corresponding endpoint might be named order - created - consumer.

            config.SetKebabCaseEndpointNameFormatter();
            if (assembly != null)
                config.AddConsumers(assembly);

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                configurator.ConfigureEndpoints(context);

                // What Does configurator.ConfigureEndpoints(context) Do?
                //Automatic Endpoint Configuration: This method scans the application for all registered consumers(message handlers) and sets up the necessary endpoints(queues) for them in the message broker(like RabbitMQ).
                //Context: The context parameter provides the configuration context, which includes information about the registered consumers and other configuration settings.

                //Why Is It Useful?
                //Convenience: You don’t have to manually specify the queue or topic names for each consumer. MassTransit handles it for you.
                //Consistency: Ensures that all consumers are properly connected to their respective queues, following the naming conventions and other settings you’ve defined.


                //Imagine you have multiple consumers in your application, each responsible for handling different types of messages. Without ConfigureEndpoints, you would need to configure each consumer’s endpoint individually. With ConfigureEndpoints, MassTransit does this automatically.
                // Host Configuration: Sets up the connection to RabbitMQ using the host, username, and password from the configuration.
                //Configure Endpoints: configurator.ConfigureEndpoints(context); scans for all consumers that have been registered(e.g., using config.AddConsumers(assembly)) and automatically configures the corresponding queues for them in RabbitMQ.

                //Simplified Explanation
                // Think of configurator.ConfigureEndpoints(context); as a magic wand that goes through all the message handlers(consumers) in your application and makes sure each one has a properly set up mailbox(queue) to receive messages from. You don't need to manually create and connect each mailbox; the magic wand does it for you, ensuring everything is correctly configured and ready to go.


                // Summary
                // Purpose: Automatically configures queues (or topics) for all registered consumers.
                // Benefit: Simplifies setup, ensures consistency, and reduces the chance of configuration errors.
                // Usage: Included in the MassTransit configuration to handle the setup of message endpoints in the message broker.

            });



        });
        return services;
    }
}
