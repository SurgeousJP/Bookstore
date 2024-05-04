using System.Text.Json.Serialization;

namespace EventBus.Messaging.Events
{
    public class IntegrationEvent
    {
        [JsonInclude]
        public Guid Id { get; set; }

        [JsonInclude]
        public DateTime CreationDate { get;  set; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationEvent(Guid id , DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
