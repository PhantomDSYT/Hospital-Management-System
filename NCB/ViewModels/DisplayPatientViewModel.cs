using HMS.Models;
using System.Collections.Generic;

namespace HMS.ViewModels
{
    public class DisplayPatientViewModel : EditPatientViewModel
    {
        public List<Visitations> Visits { get; set; }
    }
}
