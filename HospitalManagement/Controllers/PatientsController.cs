using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Models.HospitalModel;
using HospitalManagement.Models;

namespace HospitalManagement.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PatientDbContext _context;

        public PatientsController(PatientDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
              return _context.Patients != null ? 
                          View(await _context.Patients.ToListAsync()) :
                          Problem("Entity set 'PatientDbContext.Patients'  is null.");
        }
        // Action pour la recherche d'un patient par Nom
        public IActionResult SearchbySurname(string nom)
        {
            var patients = _context.Patients.ToList();
            if (String.IsNullOrEmpty(nom)) return View(patients);
            var s = _context.Patients.Where(x => x.Nom.Contains(nom)).ToList();
            return View(s.ToList()); ;
        }

        // Action pour la recherche d'un patient par Nom
        public IActionResult SearchbyName(string prenom)
        {
            var patients = _context.Patients.ToList();
            if (String.IsNullOrEmpty(prenom)) return View(patients);
            var s = _context.Patients.Where(x => x.Prenom.Contains(prenom)).ToList();
            return View(s.ToList()); ;
        }

        public IActionResult SearchBy2(string nom, string prenom)
        {
            var patients = _context.Patients.AsQueryable();
            ViewBag.Patients = patients.Select(p => p.Nom).Distinct().ToList();
            if (nom != "All")
            {
                patients = patients.Where(x => x.Nom == nom);
            }
            if (!String.IsNullOrEmpty(prenom))
            {
                patients = patients.Where(x => x.Prenom.Contains(prenom));
            }
            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            //var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
            //ViewBag.Genres = new SelectList(genres);
            //return View();  
            var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString()
            }).ToList();

            ViewBag.Genres = genres;
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,DateNaissance,Adresse,Email,NumeroTelephone,Genre")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == patient.Genre
            }).ToList();

            ViewBag.Genres = genres;
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,DateNaissance,Adresse,Email,NumeroTelephone,Genre")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Patients == null)
            {
                return Problem("Entity set 'PatientDbContext.Patients'  is null.");
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
          return (_context.Patients?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public IActionResult MesDossiersMedicaux(int id)
        {
            var patientDossiers = _context.DossiersMedicaux
                .Include(d => d.Patient)
                .Include(d => d.Medecin)
                .Include(d => d.RendezVous)
                .Where(d => d.PatientId == id)
                .ToList();

            return View(patientDossiers);
        }




    }
}
