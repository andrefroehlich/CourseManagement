using CourseManagement.Application.ReadModels;
using CourseManagement.Application.Services;
using Marten;

namespace CourseManagement.Infrastructure.Marten;

public class MartenCourseReadModelRepository(IDocumentStore store) : ICourseReadModelRepository
{
    public async Task<CourseDetailReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var session = await store.LightweightSerializableSessionAsync(cancellationToken);
        var result = await session.LoadAsync<CourseDetailReadModel>(id, cancellationToken);
        
        if (result == null)
        {
            throw new InvalidOperationException($"No document with id {id}.");
        }

        return result;
    }

    public async Task<IEnumerable<CourseOverviewSelfProjectedReadModel>> GetAllAsync(CancellationToken cancellation)
    {
        await using var session = await store.LightweightSerializableSessionAsync(cancellation);
        return await session.Query<CourseOverviewSelfProjectedReadModel>().ToListAsync(cancellation);
    }

    public async Task<CourseHistoryReadModel> GetHistoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var session = await store.LightweightSerializableSessionAsync(cancellationToken);
        var result = await session.LoadAsync<CourseHistoryReadModel>(id, cancellationToken);
        
        if (result == null)
        {
            throw new InvalidOperationException($"No document with id {id}.");
        }
        
        return result;
    }
}