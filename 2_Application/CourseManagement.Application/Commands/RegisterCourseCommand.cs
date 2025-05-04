using CourseManagement.Application.Services;
using CourseManagement.Domain.CourseAggregate;

namespace CourseManagement.Application.Commands;

public record RegisterCourseCommand(
    string Name,
    Guid CourseId, 
    string TeacherName, 
    DateTime CourseDate, 
    string Room, 
    int MaxParticipants);

public class RegisterCourseCommandHandler(IAggregateRepository repository)
{
    public async Task Handle(RegisterCourseCommand command, CancellationToken cancellationToken)
    {
        var course = Course.Register(
            command.Name,
            command.CourseId,
            command.TeacherName,
            command.CourseDate,
            command.Room,
            command.MaxParticipants);
        
        await repository.StoreAsync(course, cancellationToken);
    }
}