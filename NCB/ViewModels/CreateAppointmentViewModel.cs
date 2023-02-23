using HMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class CreateAppointmentViewModel
    {
        [Display(Name = "Appointment ID")]
        public int AppointmentID { get; set; }

        [Display(Name = "Patient ID")]
        public int PatientID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [DataType(DataType.Text)]
        public string Doctor { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
