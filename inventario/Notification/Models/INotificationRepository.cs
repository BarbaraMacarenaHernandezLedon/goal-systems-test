using inventario.Item.Models;
using System.Collections.Generic;

namespace inventario.Notification.Models
{
    public interface INotificationRepository
    {
        string Add(ItemModel item, string type);
        List<NotificationModel> GetAll();
    }
}
