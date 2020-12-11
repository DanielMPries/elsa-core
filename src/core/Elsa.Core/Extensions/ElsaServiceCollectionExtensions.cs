using System;
using Elsa;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Activities.Signaling;
using Elsa.Activities.Signaling.Services;
using Elsa.Activities.Workflows;
using Elsa.ActivityProviders;
using Elsa.ActivityTypeProviders;
using Elsa.Builders;
using Elsa.Consumers;
using Elsa.Converters;
using Elsa.Expressions;
using Elsa.Extensions;
using Elsa.HostedServices;
using Elsa.Mapping;
using Elsa.Metadata;
using Elsa.Metadata.Handlers;
using Elsa.Runtime;
using Elsa.Serialization;
using Elsa.Services;
using Elsa.Triggers;
using Elsa.WorkflowProviders;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NodaTime;
using Rebus.ServiceProvider;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ElsaServiceCollectionExtensions
    {
        public static IServiceCollection AddElsaCore(
            this IServiceCollection services,
            Action<ElsaOptions>? configure = default)
        {
            var options = new ElsaOptions(services);
            configure?.Invoke(options);

            services
                .AddSingleton(options)
                .AddSingleton(options.DistributedLockProviderFactory)
                .AddSingleton(options.SignalFactory)
                .AddSingleton(options.StorageFactory);

            options.AddWorkflowsCore();
            options.AddMediatR();
            options.AddAutoMapper();

            return services;
        }

        public static IServiceCollection AddActivity<T>(this IServiceCollection services) where T : class, IActivity =>
            services
                .AddTransient<T>()
                .AddTransient<IActivity>(sp => sp.GetRequiredService<T>());

        public static IServiceCollection AddWorkflow<T>(this IServiceCollection services) where T : class, IWorkflow =>
            services
                .AddSingleton<T>()
                .AddSingleton<IWorkflow>(sp => sp.GetRequiredService<T>());

        public static IServiceCollection AddWorkflow(this IServiceCollection services, IWorkflow workflow) =>
            services
                .AddSingleton(workflow.GetType(), workflow)
                .AddTransient(sp => workflow);

        public static IServiceCollection StartWorkflow<T>(this IServiceCollection services) where T : class, IWorkflow => services.AddHostedService<StartWorkflow<T>>();

        private static IServiceCollection AddMediatR(this ElsaOptions options) => options.Services.AddMediatR(mediatr => mediatr.AsScoped(), typeof(IActivity));

        private static ElsaOptions AddWorkflowsCore(this ElsaOptions configuration)
        {
            var services = configuration.Services;
            services.TryAddSingleton<IClock>(SystemClock.Instance);

            services
                .AddLogging()
                .AddLocalization()
                .AddMemoryCache()
                .AddSingleton<IIdGenerator, IdGenerator>()
                .AddSingleton(sp => sp.GetRequiredService<ElsaOptions>().CreateJsonSerializer(sp))
                .AddSingleton<IContentSerializer, DefaultContentSerializer>()
                .AddSingleton<TypeJsonConverter>()
                .TryAddProvider<IExpressionHandler, LiteralHandler>(ServiceLifetime.Singleton)
                .TryAddProvider<IExpressionHandler, VariableHandler>(ServiceLifetime.Singleton)
                .AddScoped<IExpressionEvaluator, ExpressionEvaluator>()
                .AddScoped<IWorkflowRegistry, WorkflowRegistry>()
                .AddScoped<IWorkflowInstanceManager, WorkflowInstanceManager>()
                .AddSingleton<IActivityActivator, ActivityActivator>()
                .AddScoped<IWorkflowRunner, WorkflowRunner>()
                .AddScoped<IWorkflowTriggerInterruptor, WorkflowTriggerInterruptor>()
                .AddSingleton<IActivityDescriber, ActivityDescriber>()
                .AddSingleton<IWorkflowFactory, WorkflowFactory>()
                .AddSingleton<IActivityFactory, ActivityFactory>()
                .AddSingleton<IWorkflowBlueprintMaterializer, WorkflowBlueprintMaterializer>()
                .AddSingleton<IWorkflowBlueprintReflector, WorkflowBlueprintReflector>()
                .AddScoped<IWorkflowSelector, WorkflowSelector>()
                .AddScoped<IWorkflowPublisher, WorkflowPublisher>()
                .AddScoped<IWorkflowContextManager, WorkflowContextManager>()              
                .AddStartupRunner()
                .AddSingleton<IActivityTypeService, ActivityTypeService>()
                .AddSingleton<IActivityTypeProvider, TypeBasedActivityProvider>()
                .AddWorkflowProvider<ProgrammaticWorkflowProvider>()
                .AddWorkflowProvider<StorageWorkflowProvider>()
                .AddTransient<IWorkflowBuilder, WorkflowBuilder>()
                .AddTransient<ICompositeActivityBuilder, CompositeActivityBuilder>()
                .AddTransient<Func<IWorkflowBuilder>>(sp => sp.GetRequiredService<IWorkflowBuilder>)
                .AddAutoMapperProfile<NodaTimeProfile>()
                .AddAutoMapperProfile<CloningProfile>()
                .AddSingleton<ICloner, AutoMapperCloner>()
                .AddNotificationHandlers(typeof(ElsaServiceCollectionExtensions))
                .AddSingleton<ServiceBusFactory>()
                .AddSingleton<IServiceBusFactory, ServiceBusFactory>()
                .AddSingleton<ICommandSender, CommandSender>()
                .AddSingleton<IEventPublisher, EventPublisher>()
                .AutoRegisterHandlersFromAssemblyOf<RunWorkflowConsumer>()
                .AddMetadataHandlers()
                .AddCoreActivities();

            return configuration;
        }

        private static IServiceCollection AddMetadataHandlers(this IServiceCollection services) =>
            services
                .AddSingleton<IActivityPropertyOptionsProvider, SelectOptionsProvider>();

        private static IServiceCollection AddCoreActivities(this IServiceCollection services) =>
            services
                .AddActivity<CompositeActivity>()
                .AddActivity<Inline>()
                .AddActivity<Finish>()
                .AddActivity<For>()
                .AddActivity<ForEach>()
                .AddActivity<ParallelForEach>()
                .AddActivity<Fork>()
                .AddActivity<IfElse>()
                .AddActivity<Join>()
                .AddActivity<Switch>()
                .AddActivity<While>()
                .AddActivity<Correlate>()
                .AddActivity<SetVariable>()
                .AddActivity<SetContextId>()
                .AddActivity<ReceiveSignal>()
                .AddActivity<SendSignal>()
                .AddActivity<RunWorkflow>()
                .AddActivity<InterruptTrigger>()
                .AddScoped<ISignaler, Signaler>()
                .AddTriggerProvider<ReceiveSignalTriggerProvider>()
                .AddTriggerProvider<RunWorkflowTriggerProvider>();
    }
}