using inventario.Item.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace inventario.Notification.Models
{
    public class NotificationRepository : INotificationRepository
    {
        // Create a "ConcurrentDictionary <ItemId, NotificationModel>" structure to store the notifications in memory.
        private static ConcurrentDictionary<string, NotificationModel> _notifications = new ConcurrentDictionary<string, NotificationModel>();

        // Definition of notifications methods.
        public string Add(ItemModel item, string type) 
        {
            NotificationModel notification = new NotificationModel()
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Item = item,
                NotificationDate = System.DateTime.Now
            };

            _notifications[notification.Id] = notification;

            // Here you should make a call to a azure function that notifies the required process
            // about what happened with this item.

            return notification.Id;
        }
        public List<NotificationModel> GetAll()
        {
            return _notifications.Values.ToList();
        }
    }
}
