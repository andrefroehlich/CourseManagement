namespace CourseManagement.Domain.CourseAggregate.Events;

public record WaitingListWithdrawn(string Participant, DateTime WithdrawnAt);