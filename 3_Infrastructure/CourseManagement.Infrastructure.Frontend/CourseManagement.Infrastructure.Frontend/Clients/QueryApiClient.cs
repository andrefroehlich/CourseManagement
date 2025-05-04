using CourseManagement.Application.ReadModels;

namespace CourseManagement.Infrastructure.Frontend.Clients;

public class QueryApiClient(HttpClient httpClient)
{
    public async Task<IQueryable<CourseOverviewSelfProjectedReadModel>?> GetAllCoursesAsync(CancellationToken cancellationToken = default)
    {
        List<CourseOverviewSelfProjectedReadModel>? courses = [];
        
        await foreach (var course in httpClient.GetFromJsonAsAsyncEnumerable<CourseOverviewSelfProjectedReadModel>(
                           "courses", cancellationToken))
        {
            courses.Add(course!);
        }

        return courses?.AsQueryable();
    }

    public async Task<CourseDetailReadModel?> GetCourseByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<CourseDetailReadModel>($"courses/{id}", cancellationToken);
    }
    
    public async Task<CourseHistoryReadModel?> GetCourseHistoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<CourseHistoryReadModel>($"courses/{id}/history", cancellationToken);
    }
}