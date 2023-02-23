using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class PatientSearchViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }

        [Display(Name = "Patient ID")]
        public int PatientID { get; set; }
    }
}
