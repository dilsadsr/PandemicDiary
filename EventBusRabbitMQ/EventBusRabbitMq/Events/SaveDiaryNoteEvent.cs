using System;
namespace EventBusRabbitMq.Events
{
    public class SaveDiaryNoteEvent
    {
        public Guid RequestID { get; set; }

        public string PersonName { get; set; }

        public string Note { get; set; }
    }
}
