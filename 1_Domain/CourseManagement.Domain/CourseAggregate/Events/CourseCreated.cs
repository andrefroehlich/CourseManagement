namespace CourseManagement.Domain.CourseAggregate.Events;

public record CourseCreated(
    string Name,
    Guid CourseId,
    string TeacherName,
    DateTime CourseDate,
    string Room,
    int MaxParticipants);