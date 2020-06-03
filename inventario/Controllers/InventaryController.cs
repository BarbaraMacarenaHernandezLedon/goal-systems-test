using Microsoft.AspNetCore.Mvc;
using inventario.Item.Models;
using inventario.Notification;
using inventario.Notification.Models;
using Microsoft.AspNetCore.Cors;

namespace inventario.Controllers
{
    [EnableCors("ExamplePolicy")]
    [Route("api/[controller]")]
    [ApiController]

    //To do: In this controller, user checks to access the API should be done
    // through "Microsoft.AspNetCore.Authorization".
    public class InventaryController : Controller
    {
        private IItemRepository _item;
        private INotificationRepository _notification;

        public InventaryController(IItemRepository item, INotificationRepository notification)
        {
            _item = item;
            _notification = notification;
        }

        // Item methods.
        [HttpGet("{itemid}")]
        public IActionResult GetItemById([FromRoute] string itemid)
        {
            ItemModel item = _item.Find(itemid);

            if (item == null)
            {
                return NotFound("El elemento no se encuentra en el inventario");
            }

            return Ok(item);
        }

        [HttpGet]
        public IActionResult GetAllItems()
        {
            return Ok(_item.GetAll());
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] ItemModel item)
        {
            // The method "isValid" was created to carry out all the verifications 
            // that are necessary on an item before being added to the inventary.
            var invalidMessage = _item.IsValid(item);
            if (invalidMessage != null)
            {
                return BadRequest(invalidMessage);
            }

            string itemId = _item.Add(item);
            return Ok(itemId);
        }

        [HttpDelete("{itemid}")]
        public IActionResult RemoveItem([FromRoute] string itemid)
        {
            if (itemid == null || itemid == string.Empty)
            {
                return BadRequest("Para que elemento sea eliminado es necesario el id del mismo");
            }

            var item = _item.Remove(itemid);

            if (item == null)
            {
                return NotFound("¡No se pudo eliminar!. El elemento no existe en el inventario");
            }

            // Once the item is removed, a new notification is added.
            _notification.Add(item, NotificationTypes.Remove);

            return Ok(itemid);
        }

        // Notifications methods.
        [HttpGet("notifications")]
        public IActionResult GetAllNotification()
        {
            return Ok(_notification.GetAll());
        }
    }
}
