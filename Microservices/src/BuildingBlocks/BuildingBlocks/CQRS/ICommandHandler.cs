using MediatR;

namespace BuildingBlocks.CQRS;

// in => you can use a base type in a position where a derived type is expected.
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
{

}
