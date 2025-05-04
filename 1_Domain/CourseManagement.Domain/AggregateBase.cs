using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using ReflectionMagic;

namespace CourseManagement.Domain;

public abstract class AggregateBase<TId> : Entity<TId> where TId : IComparable<TId>
{
    public long Version { get; protected set; }
    
    protected AggregateBase()
    {
    }

    protected AggregateBase(TId id) : base(id)
    {
    }

    [JsonIgnore]
    private readonly List<object> _uncommittedEvents = [];
    
    public IEnumerable<object> GetUncommittedEvents() => _uncommittedEvents;
    
    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();
    
    protected void RaiseDomainEvent(object @event)
    {
        this.AsDynamic().Apply(@event);
        _uncommittedEvents.Add(@event);
    }
}