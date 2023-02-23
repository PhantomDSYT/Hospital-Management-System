using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class Patient
    {
        [Key, Required]
        public int PatientID { get; set; }

        [Required, Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required, DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required, Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required, Display(Name = "Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        public string TelephoneNumber { get; set; }

        public string PatientImage { get; set; }

        [Display(Name = "Last Visit"), DataType(DataType.Date)]
        public DateTime LastVisit { get; set; }

        [Required, DataType(DataType.MultilineText)]
        [Display(Name = "Known Allergies")]
        public string Allergies { get; set; }
    }
}
