using System;
using System.ComponentModel.DataAnnotations;

namespace inventario.Item.Models
{
    public class ItemModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La fecha de caducidad es requeridad")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "El tipo es requerido")]
        public string Type { get; set; }
    }
}
