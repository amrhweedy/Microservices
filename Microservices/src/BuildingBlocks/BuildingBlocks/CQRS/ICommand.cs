﻿using MediatR;

namespace BuildingBlocks.CQRS;

// if there is no return or response 
public interface ICommand : ICommand<Unit>
{

}
// if there is a return or response  , out => to pass derived types 
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}


