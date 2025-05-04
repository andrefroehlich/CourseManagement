using CourseManagement.Application.ReadModels;
using CourseManagement.Application.Services;

namespace CourseManagement.Application.Queries;

public record GetCourseHistoryByIdQuery(Guid CourseId);

public class GetCourseHistoryByIdQueryHandler(ICourseReadModelRepository repository)
{
    public async Task<CourseHistoryReadModel> Handle(GetCourseHistoryByIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetHistoryByIdAsync(query.CourseId, cancellationToken);
    }
}