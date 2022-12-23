namespace EventBus.Messages.Events;

/// <summary>
/// Every Message event will inherit from this base event
/// it contains common members
/// </summary>
public class IntegrationBaseEvent
{
  public IntegrationBaseEvent()
  {
    Id = Guid.NewGuid();
    CreationDate = DateTime.UtcNow;
  }

  public IntegrationBaseEvent(Guid id, DateTime creationDate)
  {
    Id = id;
    CreationDate = creationDate;
  }

  public Guid Id { get; private set; }
  public DateTime CreationDate { get; private set; }
}