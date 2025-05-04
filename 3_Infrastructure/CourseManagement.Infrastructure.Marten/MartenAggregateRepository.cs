using CourseManagement.Application.Services;
using CourseManagement.Domain;
using Marten;

namespace CourseManagement.Infrastructure.Marten;

public class MartenAggregateRepository(IDocumentStore store) : IAggregateRepository
{
    public async Task<TAggregate> LoadAsync<TAggregate>(Guid id, int? version = null, CancellationToken cancellationToken = default) where TAggregate : AggregateBase<Guid>
    {
        await using var session = await store.LightweightSerializableSessionAsync(token: cancellationToken);
        var aggregate = await session.Events.AggregateStreamAsync<TAggregate>(id, version ?? 0, token: cancellationToken);
        return aggregate ?? throw new InvalidOperationException($"No aggregate by id {id}.");
    }

    public async Task StoreAsync(AggregateBase<Guid> aggregate, CancellationToken cancellationToken)
    {
        await using var session = await store.LightweightSerializableSessionAsync(token: cancellationToken);
        var events = aggregate.GetUncommittedEvents().ToArray();
        session.Events.Append(aggregate.Id, aggregate.Version, events);
        await session.SaveChangesAsync(cancellationToken);
        aggregate.ClearUncommittedEvents();
    }
}