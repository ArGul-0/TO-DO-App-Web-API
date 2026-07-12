namespace ToDoApp.Application.Interfaces
{
    /// <summary>
    /// Represents a unit of work that groups multiple operations that should be
    /// committed as a single atomic action to the underlying data store.
    /// Implementations coordinate repository operations and persist changes.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Asynchronously persists all changes made within the current unit of work
        /// to the underlying data store.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// If cancellation is requested the operation should stop and throw an <see cref="OperationCanceledException"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the underlying store.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the operation is canceled via <paramref name="cancellationToken"/>.
        /// </exception>
        /// <remarks>
        /// Typical implementations delegate to an ORM (for example DbContext.SaveChangesAsync)
        /// and should ensure transactional semantics so that either all changes are committed
        /// or none are when an error occurs.
        /// </remarks>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
