using CourseManagement.Domain;

namespace CourseManagement.Application.Services;

public interface IAggregateRepository // For aggregates a generic event-sourced repository is most likely fine
{
    Task<TAggregate> LoadAsync<TAggregate>(Guid id, int? version = null, CancellationToken cancellationToken = default)
        where TAggregate : AggregateBase<Guid>;
    Task StoreAsync(AggregateBase<Guid> aggregate, CancellationToken cancellationToken);
}