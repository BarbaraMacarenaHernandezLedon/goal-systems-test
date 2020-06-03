using System.Threading.Tasks;
using System.Collections.Generic;
using inventario.Item.Models;
using inventario.Notification;
using inventario.Notification.Models;

public class ScheduleItemExpirationVerification : ScheduledItemVerification
{
    public ScheduleItemExpirationVerification()
    {
    }

    //Runs every days at 3:00h
    protected override string ScheduleRun => "0 3 * * *";

    public override Task CheckItemExpirationDate()
    {
        IItemRepository itemRepository = new ItemRepository();
        INotificationRepository notificationRepository = new NotificationRepository();
        List<ItemModel> listItem = itemRepository.GetAll();

        foreach (ItemModel item in listItem)
        {
            // If item expired, I create the notification and remove it from inventary.
            if (item.ExpirationDate < System.DateTime.Now)
            {
                notificationRepository.Add(item, NotificationTypes.Expirate);
                itemRepository.Remove(item.Id);
            }
        }

        return Task.CompletedTask;
    }
}