using CourseManagement.Application.ReadModels;
using CourseManagement.Application.Services;

namespace CourseManagement.Application.Queries;

public record GetCourseByIdQuery(Guid CourseId);

public class GetCourseByIdQueryHandler(ICourseReadModelRepository repository)
{
    public async Task<CourseDetailReadModel> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(query.CourseId, cancellationToken);
    }
}