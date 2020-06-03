using System;

namespace client.Models
{
    public class Notifications
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public Item Item { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
