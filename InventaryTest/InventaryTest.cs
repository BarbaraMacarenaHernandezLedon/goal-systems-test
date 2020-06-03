using inventario.Controllers;
using inventario.Item.Models;
using inventario.Notification.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace InventaryTest
{
    [TestClass]
    public class InventaryTest
    {
        [TestMethod]
        public void TestGetItemById()
        {
            // Given
            var item = new ItemModel
            {
                Id = "test",
                Name = "test",
                Type = "test",
                ExpirationDate = new DateTime(2022, 6, 15, 13, 45, 30)
            };
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(x => x.Find(item.Id)).Returns(item);
            var controller = new InventaryController(mockRepository.Object, null);

            // When
            IActionResult result = controller.GetItemById(item.Id);
            var getItem = result as OkObjectResult;

            //Then
            Assert.IsNotNull(getItem);
            Assert.AreEqual(getItem.StatusCode, 200);
            Assert.AreEqual(getItem.Value, item);
        }

        [TestMethod]
        public void TestNotFoundGetItemById()
        {
            // Given
            ItemModel item = null;
            string itemId = "idtest";
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(x => x.Find(itemId)).Returns(item);
            var controller = new InventaryController(mockRepository.Object,null);

            // When
            IActionResult result = controller.GetItemById(itemId);
            var getItem = result as NotFoundObjectResult;

            //Then
            Assert.IsNotNull(getItem);
            Assert.AreEqual(getItem.StatusCode, 404);
            Assert.AreEqual(getItem.Value, "El elemento no se encuentra en el inventario");
        }

        [TestMethod]
        public void TestGetAllItems()
        {
            // Given
            List<ItemModel> listItems = new List<ItemModel>();
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(listItems);
            var controller = new InventaryController(mockRepository.Object,null);

            // When
            IActionResult result = controller.GetAllItems();
            var getItem = result as OkObjectResult;

            //Then
            Assert.IsNotNull(getItem);
            Assert.AreEqual(getItem.StatusCode, 200);
        }

        [TestMethod]
        public void TestAddItem()
        {
            // Given
            var item = new ItemModel
            {
                Name = "test",
                Type = "test",
                ExpirationDate = new DateTime(2022, 6, 15, 13, 45, 30)
            };
            string messageReturn = null;
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(x => x.IsValid(item)).Returns(messageReturn);
            mockRepository.Setup(x => x.Add(item)).Returns("idtest");
            var controller = new InventaryController(mockRepository.Object,null);

            // When
            IActionResult result = controller.AddItem(item);
            var createdItem = result as OkObjectResult;

            //Then
            Assert.IsNotNull(createdItem);
            Assert.AreEqual(createdItem.Value, "idtest");
        }

        [TestMethod]
        public void TestAddNotValidItem()
        {
            // Given
            var item = new ItemModel
            {
                Name = "test",
                Type = "test",
                ExpirationDate = new DateTime(2022, 6, 15, 13, 45, 30)
            };
            var mockRepository = new Mock<IItemRepository>();
            mockRepository.Setup(x => x.IsValid(item)).Returns("messageError");
            var controller = new InventaryController(mockRepository.Object,null);

            // When
            IActionResult result = controller.AddItem(item);
            var createdItem = result as BadRequestObjectResult;
            
            //Then
            Assert.IsNotNull(createdItem);
            Assert.AreEqual(createdItem.StatusCode,400);
            Assert.AreEqual(createdItem.Value, "messageError");
        }

        [TestMethod]
        public void TestRemoveItem()
        {
            // Given
            var item = new ItemModel
            {
                Id = "idTest",
                Name = "test",
                Type = "test",
                ExpirationDate = new DateTime(2022, 6, 15, 13, 45, 30)
            }; 
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(x => x.Remove(item.Id)).Returns(item);

            var mockNotificationRepository = new Mock<INotificationRepository>();
            var controller = new InventaryController(mockItemRepository.Object, mockNotificationRepository.Object);

            // When
            IActionResult result = controller.RemoveItem(item.Id);
            var deleteItem = result as OkObjectResult;

            //Then
            Assert.IsNotNull(deleteItem);
            Assert.AreEqual(deleteItem.Value, item.Id);
        }

        [TestMethod]
        public void TestNotFoundRemoveItem()
        {
            // Given
            string itemId = "idTest";
            ItemModel item = null;

            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(x => x.Remove(itemId)).Returns(item);

            var mockNotificationRepository = new Mock<INotificationRepository>();
            var controller = new InventaryController(mockItemRepository.Object, mockNotificationRepository.Object);

            // When
            IActionResult result = controller.RemoveItem(itemId);
            var deleteItem = result as NotFoundObjectResult;

            //Then
            Assert.IsNotNull(deleteItem);
            Assert.AreEqual(deleteItem.StatusCode, 404);
            Assert.AreEqual(deleteItem.Value, "¡No se pudo eliminar!. El elemento no existe en el inventario");
        }

        [TestMethod]
        public void TestnotValidIdRemoveItem()
        {
            // Given
            var mockItemRepository = new Mock<IItemRepository>();
            var mockNotificationRepository = new Mock<INotificationRepository>();
            var controller = new InventaryController(mockItemRepository.Object, mockNotificationRepository.Object);

            // When
            IActionResult result = controller.RemoveItem(string.Empty);
            var deleteItem = result as BadRequestObjectResult;

            //Then
            Assert.IsNotNull(deleteItem);
            Assert.AreEqual(deleteItem.StatusCode, 400);
            Assert.AreEqual(deleteItem.Value, "Para que elemento sea eliminado es necesario el id del mismo");
        }

        [TestMethod]
        public void TestGetAllNotifications()
        {
            // Given
            var mockNotificationRepository = new Mock<INotificationRepository>();
            var controller = new InventaryController(null, mockNotificationRepository.Object);

            // When
            IActionResult result = controller.GetAllNotification();
            var getNotifications = result as OkObjectResult;

            //Then
            Assert.IsNotNull(getNotifications);
            Assert.AreEqual(getNotifications.StatusCode, 200);
        }
    }
}
