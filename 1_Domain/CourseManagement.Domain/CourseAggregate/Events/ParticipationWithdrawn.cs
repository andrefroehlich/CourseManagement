namespace CourseManagement.Domain.CourseAggregate.Events;

public record ParticipationWithdrawn(
    string Participant,
    DateTime WithdrawnAt
);