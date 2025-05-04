using CourseManagement.Domain.CourseAggregate.Events;

namespace CourseManagement.Domain.CourseAggregate;

public class Course : AggregateBase<Guid>
{
    public string Name { get; private set; }
    public string TeacherName { get; private set; }
    public DateTime CourseDate { get; private set; }
    public string Room { get; private set; } = null!;
    public int MaxParticipants { get; private set; }
    public List<string> Participants { get; private set; } = [];
    public List<string> WaitingList { get; private set; } = [];

    private Course()
    {
    }

    public Course(Guid id) : base(id)
    {
    }

    public static Course Register(
        string name,
        Guid courseId,
        string teacherName,
        DateTime courseDate,
        string room,
        int maxParticipants)
    {
        var course = new Course(courseId);

        var courseCreated = new CourseCreated(
            name,
            course.Id,
            teacherName,
            courseDate,
            room,
            maxParticipants);
        course.RaiseDomainEvent(courseCreated);
        
        return course;
    }

    public void RegisterParticipant(string participant)
    {
        if (Participants.Contains(participant) || WaitingList.Contains(participant))
        {
            throw new InvalidOperationException("Participant is already registered");
        }

        if (Participants.Count < MaxParticipants)
        {
            var enrolledEvent = new ParticipantEnrolled(
                participant,
                DateTime.UtcNow);
            RaiseDomainEvent(enrolledEvent);
        }
        else
        {
            var waitlistedEvent = new ParticipantWaitlisted(
                participant,
                DateTime.UtcNow);
            RaiseDomainEvent(waitlistedEvent);
        }
    }
    
    public void WithdrawParticipation(string participant)
    {
        if (!Participants.Contains(participant) && !WaitingList.Contains(participant))
        {
            throw new InvalidOperationException("Participant is not registered");       
        }

        if (Participants.Contains(participant))
        {
            var withdrawnEvent = new ParticipationWithdrawn(
                participant,
                DateTime.UtcNow);
            RaiseDomainEvent(withdrawnEvent);

            if (WaitingList.Count == 0) return;
        
            var firstWaitingParticipant = WaitingList.First();
            var movedEvent = new ParticipantMovedFromWaitingToEnrolled(
                firstWaitingParticipant,
                DateTime.UtcNow);
            RaiseDomainEvent(movedEvent);
        }

        if (WaitingList.Contains(participant))
        {
            var withdrawnEvent = new WaitingListWithdrawn(
                participant,
                DateTime.UtcNow);
            RaiseDomainEvent(withdrawnEvent);
        }

    }
    
    private void Apply(CourseCreated @event)
    {
        Id = @event.CourseId;
        Name = @event.Name;
        TeacherName = @event.TeacherName;
        CourseDate = @event.CourseDate;
        Room = @event.Room;
        MaxParticipants = @event.MaxParticipants;
        Participants = [];
        WaitingList = [];

        Version++;
    }
    
    private void Apply(ParticipantEnrolled @event)
    {
        Participants.Add(@event.Participant);

        Version++;
    }

    private void Apply(ParticipantWaitlisted @event)
    {
        WaitingList.Add(@event.Participant);

        Version++;
    }

    private void Apply(ParticipationWithdrawn @event)
    {
        Participants.Remove(@event.Participant);

        Version++;
    }
    
    private void Apply(ParticipantMovedFromWaitingToEnrolled @event)
    {
        Participants.Add(@event.Participant);
        WaitingList.Remove(@event.Participant);

        Version++;
    }
    
    public void Apply(WaitingListWithdrawn @event)
    {
        WaitingList.Remove(@event.Participant);

        Version++;
    }
}