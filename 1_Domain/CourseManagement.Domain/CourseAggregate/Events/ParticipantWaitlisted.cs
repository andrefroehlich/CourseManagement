namespace CourseManagement.Domain.CourseAggregate.Events;

public record ParticipantWaitlisted(
    string Participant, 
    DateTime WaitlistedAt);