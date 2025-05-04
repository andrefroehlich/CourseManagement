using CourseManagement.Application.ReadModels;

namespace CourseManagement.Application.Services;

public interface ICourseReadModelRepository // Prefer specific repositories, as generic repositories are often leaky abstractions
{
    Task<CourseDetailReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<CourseOverviewSelfProjectedReadModel>> GetAllAsync(CancellationToken cancellation);
    Task<CourseHistoryReadModel> GetHistoryByIdAsync(Guid id, CancellationToken cancellationToken);
}