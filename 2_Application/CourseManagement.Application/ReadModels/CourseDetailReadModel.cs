namespace CourseManagement.Application.ReadModels;

public class CourseDetailReadModel
{
    public string Name { get; set; } = String.Empty;
    public Guid Id { get; set; }
    public string TeacherName { get; set; }
    public DateTime CourseDate { get; set; }
    public string Room { get; set; } = String.Empty;
    public int MaxParticipants { get; set; }
    public List<string> Participants { get; set; } = [];
    public List<string> WaitingList { get; set; } = [];
    
    public CourseDetailReadModel()
    {
    }
}
