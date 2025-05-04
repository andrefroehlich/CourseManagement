using CourseManagement.Application.Services;
using CourseManagement.Domain.CourseAggregate;

namespace CourseManagement.Application.Commands;

public record WithdrawParticipantCommand(
    Guid CourseId, 
    string Participant);

public class WithdrawParticipantCommandHandler(IAggregateRepository repository)
{
    public async Task Handle(WithdrawParticipantCommand command, CancellationToken cancellationToken)
    {
        var course = await repository.LoadAsync<Course>(command.CourseId, cancellationToken: cancellationToken);
        course.WithdrawParticipation(command.Participant);
        await repository.StoreAsync(course, cancellationToken);
    }
}
    