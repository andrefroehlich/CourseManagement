using CourseManagement.Application.ReadModels;
using CourseManagement.Domain.CourseAggregate.Events;
using Marten.Events.Aggregation;

namespace CourseManagement.Infrastructure.Marten.Projections;

public class CourseReadModelProjection : SingleStreamProjection<CourseDetailReadModel>
{
    public CourseDetailReadModel Create(CourseCreated @event)
    {
        return new CourseDetailReadModel
        {
            Name = @event.Name,
            Id = @event.CourseId,
            TeacherName = @event.TeacherName,
            CourseDate = @event.CourseDate,
            Room = @event.Room,
            MaxParticipants = @event.MaxParticipants
        };
    }

    public void Apply(ParticipantEnrolled @event, CourseDetailReadModel courseDetail)
    {
        courseDetail.Participants.Add(@event.Participant);
    }
    
    public void Apply(ParticipantWaitlisted @event, CourseDetailReadModel courseDetail)
    {
        courseDetail.WaitingList.Add(@event.Participant);
    }
    
    public void Apply(ParticipationWithdrawn @event, CourseDetailReadModel courseDetail)
    {
        courseDetail.Participants.Remove(@event.Participant);
    }
    
    public void Apply(ParticipantMovedFromWaitingToEnrolled @event, CourseDetailReadModel courseDetail)
    {
        courseDetail.Participants.Add(@event.Participant);
        courseDetail.WaitingList.Remove(@event.Participant);
    }

    public void Apply(WaitingListWithdrawn @event, CourseDetailReadModel courseDetail)
    {
        courseDetail.WaitingList.Remove(@event.Participant);
    }
}