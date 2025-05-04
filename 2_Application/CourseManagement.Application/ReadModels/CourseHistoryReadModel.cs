using CourseManagement.Domain.CourseAggregate.Events;

namespace CourseManagement.Application.ReadModels;

public record HistoryItem(DateTime Date, string Event);

public class CourseHistoryReadModel
{
    public Guid Id { get; set; }
    public List<HistoryItem> History { get; set; } = [];
}