using CourseManagement.Domain.CourseAggregate.Events;

namespace CourseManagement.Application.ReadModels;

public class CourseOverviewSelfProjectedReadModel
{
    public string Name { get; set; } = String.Empty;
    public Guid Id { get; set; }
    public string TeacherName { get; set; } = String.Empty;
    public DateTime CourseDate { get; set; }
    public string Room { get; set; } = String.Empty;
    public int MaxParticipants { get; set; }

    public int NumberOfParticipants { get; set; } = 0;
    
    public int NumberOfWaitingListedParticipants { get; set; } = 0;
    
    public int NumberOfFreePlaces { get; set; } = 0;
    
    public bool IsFull { get; set; } = false;
    
    public CourseOverviewSelfProjectedReadModel()
    {}

    public void Apply(CourseCreated @event)
    {
        Id = @event.CourseId;
        Name = @event.Name;
        TeacherName = @event.TeacherName;
        CourseDate = @event.CourseDate;
        Room = @event.Room;
        MaxParticipants = @event.MaxParticipants;
        NumberOfFreePlaces = @event.MaxParticipants;
    }
    
    public void Apply(ParticipantEnrolled @event)
    {
        NumberOfParticipants++;
        NumberOfFreePlaces--;
    }
    
    public void Apply(ParticipantWaitlisted @event)
    {
        NumberOfWaitingListedParticipants++;
    }
    
    public void Apply(ParticipationWithdrawn @event)
    {
        NumberOfParticipants--;
        NumberOfFreePlaces++;
    }
    
    public void Apply(ParticipantMovedFromWaitingToEnrolled @event)
    {
        NumberOfParticipants++;
        NumberOfWaitingListedParticipants--;
        NumberOfFreePlaces--;
    }
    
    public void Apply(WaitingListWithdrawn @event)
    {
        NumberOfWaitingListedParticipants--;
    }
}