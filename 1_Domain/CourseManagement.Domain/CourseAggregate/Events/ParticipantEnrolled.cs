namespace CourseManagement.Domain.CourseAggregate.Events;

public record ParticipantEnrolled(
    string Participant,
    DateTime EnrolledAt);