using CourseManagement.Application.Commands;

namespace CourseManagement.Infrastructure.Frontend.Clients;

public class CommandApiClient(HttpClient httpClient)
{
    public async Task RegisterCourseAsync(RegisterCourseCommand command, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync("courses", command, cancellationToken);
    }
    
    public async Task RegisterParticipantAsync(RegisterParticipantCommand command, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync($"courses/{command.CourseId}/participant-registrations", command, cancellationToken);
    }
    
    public async Task WithdrawParticipantAsync(WithdrawParticipantCommand command, CancellationToken cancellationToken = default)
    {
        await httpClient.PostAsJsonAsync($"courses/{command.CourseId}/participant-withdrawals", command, cancellationToken);
    }
}