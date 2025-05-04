using CourseManagement.Application.Services;
using CourseManagement.Domain.CourseAggregate;

namespace CourseManagement.Application.Commands;

public record RegisterParticipantCommand(
    Guid CourseId, 
    string Participant);

public class RegisterParticipantCommandHandler(IAggregateRepository repository)
{
    public async Task Handle(RegisterParticipantCommand command, CancellationToken cancellationToken)
    {
        var course = await repository.LoadAsync<Course>(command.CourseId, cancellationToken: cancellationToken);
        course.RegisterParticipant(command.Participant);
        await repository.StoreAsync(course, cancellationToken);
    }
}