using HMS.Data;
using HMS.Models;
using HMS.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize(Roles = "Admin, Doctor, Registrar, Receptionist")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public AppointmentController(ApplicationDbContext context,
                                     UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var docs = await userManager.GetUsersInRoleAsync("Doctor");
            List<SelectListItem> list = new List<SelectListItem>();


            foreach(var item in docs)
                list.Add(new SelectListItem { Text = "Dr. " + item.LastName, Value = item.Id });

            SelectList doctors = new SelectList(list, "Value", "Text");

            ViewBag.DoctorSelection = doctors;

            CreateAppointmentViewModel model = new CreateAppointmentViewModel { Date = System.DateTime.Now };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            if(ModelState.IsValid)
            {
                var patient = await _context.Patient.FindAsync(model.PatientID);

                var visits = from p in _context.Appointment select p;

                visits = visits.Where(p => p.Date == model.Date && p.Time == model.Time);

                foreach(var item in visits)
                {
                    if(item.Time == model.Time && item.Doctor == model.Doctor)
                    {
                        ModelState.AddModelError(string.Empty, "Sorry but that doctor is already booked for that time");
                    }
                }
                 
                var doctor = await userManager.FindByIdAsync(model.Doctor);

                if(patient != null)
                {

                    Appointment appointment = new Appointment
                    {
                        PatientName = patient.FirstName + " " + patient.LastName,
                        PatientID = model.PatientID,
                        Date = model.Date,
                        Time = model.Time,
                        Doctor = model.Doctor,
                        DoctorName = "Dr. " + doctor.LastName,
                        Description = model.Description,
                    };

                    await _context.Appointment.AddAsync(appointment);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Patient does not exist");
            }
            ViewBag.DoctorSelection = ViewBag.DoctorSelection;
            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("Doctor"))
                 RedirectToAction("ViewAppointments");
            return View(await _context.Appointment.ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Appointment with Id = {id} cannot be found";
                return View("NotFound");
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                ViewBag.ErrorMessage = $"Appointment with Id = {id} cannot be found";
                return View("NotFound");
            }

            return View();
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ViewAppointments()
        {
            var visits = from p in _context.Appointment select p;
            var userId = userManager.GetUserId(User);

            visits = visits.Where(p => p.Doctor == userId);

            return View(await visits.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var patient = await _context.Appointment
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        public async Task<IActionResult> Complete(Appointment model)
        {
            var patient = await _context.Patient.FindAsync(model.PatientID);
            patient.LastVisit = model.Date;
            Visitations visit = new Visitations
            {
                Date = model.Date,
                Description = model.Description,
                Patient = model.PatientID
            };

            _context.Patient.Update(patient);
            await _context.Visitations.AddAsync(visit);
            await _context.SaveChangesAsync();

            await DeleteConfirmed(model.AppointmentID);

            return RedirectToAction("index", "Home");
        }
    }
}
