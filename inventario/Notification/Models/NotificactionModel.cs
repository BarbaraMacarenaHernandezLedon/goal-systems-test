using System;
using inventario.Item.Models;
using System.ComponentModel.DataAnnotations;

namespace inventario.Notification.Models
{
    public class NotificationModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El tipo de notificacion es requerido")]
        public string Type { get; set; }

        [Required(ErrorMessage = "El elemento es requerido")]
        public ItemModel Item { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
