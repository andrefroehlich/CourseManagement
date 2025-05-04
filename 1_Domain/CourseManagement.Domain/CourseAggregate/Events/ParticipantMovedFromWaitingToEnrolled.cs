namespace CourseManagement.Domain.CourseAggregate.Events;

public record ParticipantMovedFromWaitingToEnrolled(
    string Participant,
    DateTime EnrolledAt
);