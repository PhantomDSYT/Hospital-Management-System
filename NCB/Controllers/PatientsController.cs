using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using HMS.Models;
using HMS.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize(Roles ="Admin, Doctor, Nurse, Receptionist, Registrar, Pharmacist")]
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public PatientsController(ApplicationDbContext context,
                                  IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patient.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Patient with Id = {id} cannot be found";
                return View("NotFound");
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                ViewBag.ErrorMessage = $"Patient with Id = {id} does not exist";
                return View("NotFound");
            }

            var visits = from v in _context.Visitations select v;

            visits = visits.Where(s => s.Patient == id);

            DisplayPatientViewModel model = new DisplayPatientViewModel
            {
                PatientID = patient.PatientID,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Gender = patient.Gender,
                DateofBirth = patient.DateofBirth,
                Address = patient.Address,
                TelephoneNumber = patient.TelephoneNumber,
                PhotoPath = patient.PatientImage,
                Allergies = patient.Allergies,
                Visits = await visits.ToListAsync()
            };

            return View(model);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniquefilename = ProcessUploadedFile(model);

                Patient patient = new Patient
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    DateofBirth = model.DateofBirth,
                    Address = model.Address,
                    TelephoneNumber = model.TelephoneNumber,
                    Allergies = model.Allergies,
                    PatientImage = uniquefilename
                };
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private string ProcessUploadedFile(CreatePatientViewModel patient)
        {
            string uniquefilename = null;
            if(patient != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images", "patients");
                uniquefilename = Guid.NewGuid().ToString() + "_" + patient.PatientPicture.FileName;
                string filepath = Path.Combine(uploadsFolder, uniquefilename);
                FileStream fs = new FileStream(filepath, FileMode.Create);
                patient.PatientPicture.CopyTo(fs);
                fs.Close();
            }
            return uniquefilename;
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = $"Patient with Id = {id} cannot be found";
                return View("NotFound");
            }

            var patient = await _context.Patient.FindAsync(id);

            if (patient == null)
            {
                ViewBag.ErrorMessage = $"Patient with Id = {id} does not exist";
                return View("NotFound");
            }
            EditPatientViewModel editPatientViewModel = new EditPatientViewModel
            {
                PatientID = patient.PatientID,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Address = patient.Address,
                Gender = patient.Gender,
                DateofBirth = patient.DateofBirth,
                TelephoneNumber = patient.TelephoneNumber,
                Allergies = patient.Allergies,
                PhotoPath = patient.PatientImage
            };
            
            return View(editPatientViewModel);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPatientViewModel model)
        {
            if (!PatientExists(model.PatientID))
            {
                ViewBag.ErrorMessage = $"Patient with Id = {model.PatientID} cannot be found";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var patient = await _context.Patient.FindAsync(model.PatientID);
                    patient.FirstName = model.FirstName;
                    patient.LastName = model.LastName;
                    patient.Address = model.Address;    
                    patient.Gender = model.Gender;
                    patient.DateofBirth = model.DateofBirth;
                    patient.TelephoneNumber = model.TelephoneNumber;
                    patient.Allergies = model.Allergies;
                    if(model.PatientPicture != null)
                    {
                        if(model.PhotoPath != null)
                        {
                            string filepath = Path.Combine(hostingEnvironment.WebRootPath, "images", "patients", model.PhotoPath);
                            System.IO.File.Delete(filepath);
                        }
                        patient.PatientImage = ProcessUploadedFile(model);
                    }
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(model.PatientID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patient.FindAsync(id);

            EditPatientViewModel model = new EditPatientViewModel
            {
                PhotoPath = patient.PatientImage
            };

            if(model.PhotoPath != null)
            {
                string filepath = Path.Combine(hostingEnvironment.WebRootPath, "images", "patients", model.PhotoPath);
                System.IO.File.Delete(filepath);
            }

            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.PatientID == id);
        }

        [HttpGet]
        public IActionResult SearchPatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchPatient(PatientSearchViewModel model)
        {
            return RedirectToAction("ResultList", model);
        }

        public async Task<IActionResult> ResultList(PatientSearchViewModel model)
        {
            var patients = from p in _context.Patient select p;

            patients = patients.Where(p => p.FirstName!.Contains(model.FirstName));
            patients = patients.Where(p => p.LastName!.Contains(model.LastName));
            patients = patients.Where(p => p.DateofBirth == model.DateofBirth);

            return View(await patients.ToListAsync());
        }
    }
}
