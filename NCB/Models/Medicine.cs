using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class Medicine
    {
        [Key, Required]
        public int ID { get; set; }

        [Required, DataType(DataType.Text)]
        public string Name { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Restock { get; set; }
    }
}
