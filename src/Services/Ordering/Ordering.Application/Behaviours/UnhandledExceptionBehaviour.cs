using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours;

/// <summary>
/// Post Request to Handle any exception which has not been caught
/// used to log exceptions
/// </summary>
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  private readonly ILogger<TRequest> _logger;

  public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
  {
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    try
    {
      // this will fail if there are any errors we did not catch in the pre-request pipeline handlers
      return await next();
    }
    catch (Exception e)
    {
      var requestName = typeof(TRequest).Name;
      _logger.LogError(e, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName,
        request);
      throw;
    }
  }
}