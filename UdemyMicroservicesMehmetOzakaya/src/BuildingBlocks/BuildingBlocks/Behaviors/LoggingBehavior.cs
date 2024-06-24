﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse> (ILogger<LoggingBehavior<TRequest,TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
where TRequest : notnull, IRequest<TResponse>
where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[start] handle request= {typeof(TRequest).Name} - response = {typeof(TResponse).Name} - requestData = {request}");

        var timer = new Stopwatch();
        timer.Start();

      var response =   await next();

        timer.Stop();
        var timeTaken= timer.Elapsed;

        if (timeTaken.Seconds > 3)
            logger.LogWarning($"[performance] the request {request} took {timeTaken} seconds");


        logger.LogInformation($"[END] Handled {typeof(TRequest).Name} with {typeof(TResponse).Name}");


        return response;


    }
}
