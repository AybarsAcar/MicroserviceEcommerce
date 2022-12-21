using System.Linq.Expressions;
using Ordering.Domain.Common;

namespace Ordering.Application.Contracts.Persistence;

/// <summary>
/// Generic Repository interface
/// </summary>
public interface IAsyncRepository<T> where T : EntityBase
{
  /// <summary>
  /// Returns all the items
  /// </summary>
  /// <returns></returns>
  Task<IReadOnlyList<T>> GetAllAsync();

  /// <summary>
  /// takes the expression as a function and filter our entities using the predicate functions
  /// </summary>
  /// <param name="predicate"></param>
  /// <returns></returns>
  Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

  /// <summary>
  /// Pagination queries with configurations
  /// </summary>
  /// <param name="predicate"></param>
  /// <param name="orderBy"></param>
  /// <param name="includeString"></param>
  /// <param name="disableTracking"></param>
  /// <returns></returns>
  Task<IReadOnlyList<T>> GetAsync(
    Expression<Func<T, bool>> predicate = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string includeString = null,
    bool disableTracking = true
  );

  /// <summary>
  /// Pagination queries with configurations
  /// </summary>
  /// <param name="predicate"></param>
  /// <param name="orderBy"></param>
  /// <param name="includes">Predicate as a function format</param>
  /// <param name="disableTracking"></param>
  /// <returns></returns>
  Task<IReadOnlyList<T>> GetAsync(
    Expression<Func<T, bool>> predicate = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    List<Expression<Func<T, object>>> includes = null,
    bool disableTracking = true
  );

  /// <summary>
  /// Returns the entity by its id
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  Task<T> GetByIdAsync(int id);

  /// <summary>
  /// Creates a new entity
  /// </summary>
  /// <param name="entity"></param>
  /// <returns></returns>
  Task<T> AddAsync(T entity);

  /// <summary>
  /// Updates the entity
  /// </summary>
  /// <param name="entity"></param>
  /// <returns></returns>
  Task UpdateAsync(T entity);

  /// <summary>
  /// deletes an entity given the object
  /// </summary>
  /// <param name="entity"></param>
  /// <returns></returns>
  Task DeleteAsync(T entity);
}