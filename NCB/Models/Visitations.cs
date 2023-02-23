using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class Visitations
    {
        [Key]
        public int VisitID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int Patient { get; set; }
    }
}
