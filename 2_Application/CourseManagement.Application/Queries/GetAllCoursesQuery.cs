using CourseManagement.Application.ReadModels;
using CourseManagement.Application.Services;

namespace CourseManagement.Application.Queries;

public record GetAllCoursesQuery();

public class GetAllCoursesQueryHandler(ICourseReadModelRepository repository)
{
    public async Task<IEnumerable<CourseOverviewSelfProjectedReadModel>> Handle(GetAllCoursesQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}