using CourseManagement.Domain.CourseAggregate;
using CourseManagement.Domain.CourseAggregate.Events;

namespace CourseManagement.Domain.Tests;

public static class CourseAggregateTest
{
    public class Create
    {
        [Fact]
        public void ShouldCreateCourse()
        {
            // Arrange
            var name = "Course 1";
            var courseId = Guid.NewGuid();
            var teacherName = "John";
            var courseDate = new DateTime(2024, 10, 15);
            var room = "Room 101";
            var maxParticipants = 20;

            // Act
            var course = Course.Register(name, courseId, teacherName, courseDate, room, maxParticipants);

            // Assert - course properties
            course.Should().NotBeNull();
            course.Name.Should().Be(name);
            course.TeacherName.Should().Be(teacherName);
            course.CourseDate.Should().Be(courseDate);
            course.Room.Should().Be(room);
            course.MaxParticipants.Should().Be(maxParticipants);

            // Assert - domain event raised
            var domainEvents = course.GetUncommittedEvents().ToList();
            domainEvents.Should().ContainSingle();

            var courseCreatedEvent = domainEvents[0].Should().BeOfType<CourseCreated>().Subject;
            courseCreatedEvent.CourseId.Should().Be(course.Id);
            courseCreatedEvent.Name.Should().Be(name);
            courseCreatedEvent.TeacherName.Should().Be(teacherName);
            courseCreatedEvent.CourseDate.Should().Be(courseDate);
            courseCreatedEvent.Room.Should().Be(room);
            courseCreatedEvent.MaxParticipants.Should().Be(maxParticipants);
        }
    }

    public class RegisterParticipant
    {
        [Fact]
        public void ShouldEnrollParticipant_WhenCapacityNotExceeded()
        {
            // Arrange
            var name = "Course 1";
            var courseId = Guid.NewGuid();
            var teacherName = "John";
            var courseDate = new DateTime(2024, 10, 15);
            var room = "Room 101";
            var maxParticipants = 2;
            var participant = "Jane";

            var course = Course.Register(name, courseId, teacherName, courseDate, room, maxParticipants);

            // Act
            course.RegisterParticipant(participant);

            // Assert - participant enrolled
            course.Participants.Should().ContainSingle().Which.Should().Be(participant);
            course.WaitingList.Should().BeEmpty();

            // Assert - domain event raised
            var events = course.GetUncommittedEvents().ToList();
            events.Should().ContainSingle(evt => evt is ParticipantEnrolled);

            var enrolledEvent = events.OfType<ParticipantEnrolled>().Single();
            enrolledEvent.Participant.Should().Be(participant);
            enrolledEvent.EnrolledAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void ShouldWaitlistParticipant_WhenCapacityExceeded()
        {
            // Arrange
            var name = "Course 1";
            var courseId = Guid.NewGuid();
            var teacherName = "John";
            var courseDate = new DateTime(2024, 10, 15);
            var room = "Room 101";
            var maxParticipants = 1;
            var firstParticipant = "Jane";
            var secondParticipant = "William";

            var course = Course.Register(name, courseId, teacherName, courseDate, room, maxParticipants);
            course.RegisterParticipant(firstParticipant);

            // Act
            course.RegisterParticipant(secondParticipant);

            // Assert - first participant enrolled, second waitlisted
            course.Participants.Should().ContainSingle().Which.Should().Be(firstParticipant);
            course.WaitingList.Should().ContainSingle().Which.Should().Be(secondParticipant);

            // Assert - domain events raised: 2 events (enrolled + waitlisted)
            var events = course.GetUncommittedEvents().ToList();
            events.OfType<ParticipantEnrolled>().Should().ContainSingle(pe => pe.Participant == firstParticipant);
            events.OfType<ParticipantWaitlisted>().Should()
                .ContainSingle(pw => pw.Participant == secondParticipant);

            var waitlistedEvent = events.OfType<ParticipantWaitlisted>()
                .Single(pw => pw.Participant == secondParticipant);
            waitlistedEvent.WaitlistedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
        
        [Fact]
        public void ShouldThrow_WhenParticipantAlreadyRegisteredOrWaitlisted()
        {
            // Arrange
            var name = "Course 1";
            var courseId = Guid.NewGuid();
            var teacherName = "John";
            var courseDate = new DateTime(2024, 10, 15);
            var room = "Room 101";
            var maxParticipants = 1;
            var participant = "Jane";

            var course = Course.Register(name, courseId, teacherName, courseDate, room, maxParticipants);

            // Register the participant initially
            course.RegisterParticipant(participant);

            // Act & Assert - registering the same participant again should throw
            Action actEnrollAgain = () => course.RegisterParticipant(participant);
            actEnrollAgain.Should().Throw<InvalidOperationException>()
                .WithMessage("Participant is already registered");

            // Register another participant that goes to waiting list
            var waitingParticipant = "William";
            course.RegisterParticipant(waitingParticipant);

            // Act & Assert - registering the same waiting participant again should throw
            Action actWaitlistAgain = () => course.RegisterParticipant(waitingParticipant);
            actWaitlistAgain.Should().Throw<InvalidOperationException>()
                .WithMessage("Participant is already registered");
        }

    }
    
    public class WithdrawParticipation
    {
        [Fact]
        public void ShouldWithdrawEnrolledParticipant_AndMoveWaitingParticipantToEnrolled()
        {
            // Arrange
            var name = "Course 1";
            var courseId = Guid.NewGuid();
            var teacherName = "John";
            var courseDate = new DateTime(2024, 10, 15);
            var room = "Room 101";
            var maxParticipants = 1;
            var enrolledParticipant = "Jane";
            var waitingParticipant = "William";

            var course = Course.Register(name, courseId, teacherName, courseDate, room, maxParticipants);
            course.RegisterParticipant(enrolledParticipant);
            course.RegisterParticipant(waitingParticipant);

            course.ClearUncommittedEvents();

            // Act
            course.WithdrawParticipation(enrolledParticipant);

            // Assert
            course.Participants.Should().NotContain(enrolledParticipant);
            course.Participants.Should().Contain(waitingParticipant);
            course.WaitingList.Should().BeEmpty();

            var events = course.GetUncommittedEvents().ToList();
            events.OfType<ParticipationWithdrawn>().Should().ContainSingle(pw => pw.Participant == enrolledParticipant);
            events.OfType<ParticipantMovedFromWaitingToEnrolled>().Should().ContainSingle(pe => pe.Participant == waitingParticipant);
        }

        [Fact]
        public void ShouldWithdrawWaitingListParticipant()
        {
            // Arrange
            var name = "Course 1";
            var courseId = Guid.NewGuid();
            var teacherName = "John";
            var courseDate = new DateTime(2024, 10, 15);
            var room = "Room 101";
            var maxParticipants = 1;
            var enrolledParticipant = "Jane";
            var waitingParticipant = "William";

            var course = Course.Register(name, courseId, teacherName, courseDate, room, maxParticipants);
            course.RegisterParticipant(enrolledParticipant);
            course.RegisterParticipant(waitingParticipant);

            course.ClearUncommittedEvents();

            // Act
            course.WithdrawParticipation(waitingParticipant);

            // Assert
            course.Participants.Should().ContainSingle(p => p == enrolledParticipant);
            course.WaitingList.Should().NotContain(waitingParticipant);

            var events = course.GetUncommittedEvents().ToList();
            events.OfType<WaitingListWithdrawn>().Should().ContainSingle(wlw => wlw.Participant == waitingParticipant);
            events.OfType<ParticipationWithdrawn>().Should().BeEmpty();
            events.OfType<ParticipantMovedFromWaitingToEnrolled>().Should().BeEmpty();
        }

        
        [Fact]
        public void ShouldThrow_WhenParticipantDoesNotExist()
        {
            // Arrange
            var name = "Course 1";
            var courseId = Guid.NewGuid();
            var teacherName = "John";
            var courseDate = new DateTime(2024, 10, 15);
            var room = "Room 101";
            var maxParticipants = 2;
            var nonExistentParticipant = "NonExistent";

            var course = Course.Register(name, courseId, teacherName, courseDate, room, maxParticipants);

            // Act
            Action act = () => course.WithdrawParticipation(nonExistentParticipant);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Participant is not registered");
        }
    }
}