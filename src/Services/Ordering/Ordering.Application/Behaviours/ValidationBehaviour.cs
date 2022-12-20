using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours;

/// <summary>
/// Pre Request Handle Validation Middleware
/// collect and accumulate all the validations and run the validations validate method
/// and see if any error from the validation side, we can throw our validation error
/// </summary>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  // collect all IValidator objects from the assembly using the reflection
  // of the fluent validator, all the fluent validators in our application are collected
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
  {
    _validators = validators;
  }

  /// <summary>
  /// To handle the pipeline behaviours
  /// </summary>
  /// <param name="request"></param>
  /// <param name="next">after the behaviour we can call the next to call the next behaviour in the pipeline
  /// and if there is no behaviour it will call the actual Handle method from the IRequest class implementation</param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    // check if there any validators in the solution assembly
    if (_validators.Any())
    {
      var context = new ValidationContext<TRequest>(request);

      // return when all validations finished
      // runs all the validators and returns all the results
      var validationResults = await Task.WhenAll(
        _validators.Select(v => v.ValidateAsync(context, cancellationToken))
      );

      // check failures; and collect all the non-null failures into a list
      var failures = validationResults
        .SelectMany(r => r.Errors)
        .Where(f => f != null)
        .ToList();

      if (failures.Count > 0)
      {
        throw new ValidationException(failures);
      }
    }

    // continue to the next Handler in the request pipeline
    return await next();
  }
}