using CourseManagement.Application.ReadModels;
using CourseManagement.Domain.CourseAggregate.Events;
using Marten.Events;
using Marten.Events.Aggregation;

namespace CourseManagement.Infrastructure.Marten.Projections;

public class CourseHistoryReadModelProjection : SingleStreamProjection<CourseHistoryReadModel>
{
    public CourseHistoryReadModel Create(IEvent<CourseCreated> @event)
    {
        return new CourseHistoryReadModel
        {
            Id = @event.Data.CourseId,
            History = [ 
                new HistoryItem(
                    @event.Timestamp.UtcDateTime, 
                    "Course Created")
            ]
        };
    }

    public void Apply(ParticipantEnrolled @event, CourseHistoryReadModel courseHistory)
    {
        courseHistory.History.Add(new HistoryItem(
            @event.EnrolledAt,
            $"Participant Enrolled ({@event.Participant})"
        ));
    }

    public void Apply(ParticipantWaitlisted @event, CourseHistoryReadModel courseHistory)
    {
        courseHistory.History.Add(new HistoryItem(
            @event.WaitlistedAt,
            $"Participant Waitlisted ({@event.Participant})"));
    }

    public void Apply(ParticipationWithdrawn @event, CourseHistoryReadModel courseHistory)
    {
        courseHistory.History.Add(new HistoryItem(
            @event.WithdrawnAt,
            $"Participation Withdrawn ({@event.Participant})"));
    }

    public void Apply(ParticipantMovedFromWaitingToEnrolled @event, CourseHistoryReadModel courseHistory)
    {
        courseHistory.History.Add(new HistoryItem(
            @event.EnrolledAt,
            $"Participant moved from waiting list to enrolled ({@event.Participant})"));
    }

    public void Apply(WaitingListWithdrawn @event, CourseHistoryReadModel courseHistory)
    {
        courseHistory.History.Add(new HistoryItem(
            @event.WithdrawnAt,
            $"Waiting list place withdrawn ({@event.Participant})"));
    }
}