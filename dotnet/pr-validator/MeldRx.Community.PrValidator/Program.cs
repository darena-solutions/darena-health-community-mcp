// See https://aka.ms/new-console-template for more information

using CommandLine;
using MeldRx.Community.PrValidator.Commands;

var types = typeof(Program).Assembly.GetTypes();
var commandTypes = types.Where(x => !x.IsInterface && x.IsAssignableTo(typeof(ICommand)));
var handlerTypes = types.Where(x =>
    x.GetInterfaces()
        .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
);

await Parser
    .Default.ParseArguments(args, [.. commandTypes])
    .WithParsedAsync(
        async (obj) =>
        {
            var handlerType =
                handlerTypes.FirstOrDefault(x =>
                    x.GetInterfaces()
                        .Any(y =>
                            y.GenericTypeArguments.Length > 0
                            && y.GenericTypeArguments[0] == obj.GetType()
                        )
                )
                ?? throw new InvalidOperationException(
                    $"No handler for command type: {obj.GetType().Name}"
                );

            var handler = Activator.CreateInstance(handlerType);
            var method =
                handlerType.GetMethod(
                    nameof(ICommandHandler<VerifyServiceCollectionCommand>.HandleAsync)
                )
                ?? throw new InvalidOperationException(
                    $"Handler method could not be found for handler: {handlerType.Name}"
                );

            var task =
                (Task<bool>)(
                    method.Invoke(handler, [obj])
                    ?? throw new InvalidOperationException(
                        "Handler method invocation returned null"
                    )
                );

            var isSuccess = await task;
            Environment.Exit(isSuccess ? 0 : 1);
        }
    );
