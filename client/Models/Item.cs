using System;

namespace client.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime ExpirationDate { get; set; }
        public string Type { get; set; }
    }
}
