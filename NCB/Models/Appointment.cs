using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class Appointment
    {
        [Key, Required]
        [Display(Name = "Appointment ID")]
        public int AppointmentID { get; set; }

        [Required, Display(Name ="Patient Name")]
        [DataType(DataType.Text)]
        public string PatientName { get; set; }

        [Required, Display(Name = "Patient ID")]
        public int PatientID { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [Required, DataType(DataType.Text)]
        public string Doctor { get; set; }

        [Required, Display(Name = "Doctor Name")]
        [DataType(DataType.Text)]
        public string DoctorName { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }


    }
}
