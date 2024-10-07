using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Models.HospitalModel;
using HospitalManagement.Migrations;

namespace HospitalManagement.Controllers
{
    public class RendezVousController : Controller
    {
        private readonly PatientDbContext _context;

        public RendezVousController(PatientDbContext context)
        {
            _context = context;
        }

        // GET: RendezVous
        public async Task<IActionResult> Index()
        {
            var patientDbContext = _context.RendezVous.Include(r => r.Medecin).Include(r => r.Patient);
            return View(await patientDbContext.ToListAsync());
        }

        // GET: RendezVous/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RendezVous == null)
            {
                return NotFound();
            }

            var rendezVous = await _context.RendezVous
                .Include(r => r.Medecin)
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rendezVous == null)
            {
                return NotFound();
            }

            return View(rendezVous);
        }

        // GET: RendezVous/Create
        public IActionResult Create()
        {
            // Sélectionnez les patients avec leurs informations
            var patientsWithInfo = _context.Patients.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque patient
            var patientList = patientsWithInfo.Select(p => new { Id = p.Id, FullName = $"{p.Id} - {p.Nom} {p.Prenom}" }).ToList();

            // Sélectionnez les medecins avec leurs informations
            var medecinsWithInfo = _context.Medecins.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque medecin
            var medecinList = medecinsWithInfo.Select(m => new { Id = m.Id, FullName = $"{m.Id} - {m.Nom} {m.Prenom}" }).ToList();

            // Passez la liste à la vue via ViewBag
            ViewData["Patients"] = new SelectList(patientList, "Id", "FullName");
            ViewData["Medecins"] = new SelectList(medecinList, "Id", "FullName");
            return View();
        }

        // POST: RendezVous/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateRendezVous,Motif,PatientId,MedecinId")] RendezVous rendezVous)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rendezVous);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Sélectionnez les patients avec leurs informations
            var patientsWithInfo = _context.Patients.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque patient
            var patientList = patientsWithInfo.Select(p => new { Id = p.Id, FullName = $"{p.Id} - {p.Nom} {p.Prenom}" }).ToList();

            // Sélectionnez les medecins avec leurs informations
            var medecinsWithInfo = _context.Medecins.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque medecin
            var medecinList = medecinsWithInfo.Select(m => new { Id = m.Id, FullName = $"{m.Id} - {m.Nom} {m.Prenom}" }).ToList();

            // Passez la liste à la vue via ViewBag
            ViewData["Patients"] = new SelectList(patientList, "Id", "FullName");
            ViewData["Medecins"] = new SelectList(medecinList, "Id", "FullName");
            return View(rendezVous);
        }

        // GET: RendezVous/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RendezVous == null)
            {
                return NotFound();
            }

            var rendezVous = await _context.RendezVous.FindAsync(id);
            if (rendezVous == null)
            {
                return NotFound();
            }  // Sélectionnez les médecins avec leurs informations
            // Sélectionnez les patients avec leurs informations
            var patientsWithInfo = _context.Patients.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque patient
            var patientList = patientsWithInfo.Select(p => new { Id = p.Id, FullName = $"{p.Nom} {p.Prenom}" }).ToList();

            // Sélectionnez les médecins avec leurs informations
            var medecinsWithInfo = _context.Medecins.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque médecin
            var medecinList = medecinsWithInfo.Select(m => new { Id = m.Id, FullName = $"{m.Nom} {m.Prenom}" }).ToList();

            // Passez la liste à la vue via ViewBag
            ViewData["Patients"] = new SelectList(patientList, "Id", "FullName", rendezVous.PatientId);
            ViewData["Medecins"] = new SelectList(medecinList, "Id", "FullName", rendezVous.MedecinId);
            return View(rendezVous);
        }

        // POST: RendezVous/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateRendezVous,Motif,PatientId,MedecinId")] RendezVous rendezVous)
        {
            if (id != rendezVous.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rendezVous);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RendezVousExists(rendezVous.Id))
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
            // Sélectionnez les patients avec leurs informations
            var patientsWithInfo = _context.Patients.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque patient
            var patientList = patientsWithInfo.Select(p => new { Id = p.Id, FullName = $"{p.Nom} {p.Prenom}" }).ToList();

            // Sélectionnez les médecins avec leurs informations
            var medecinsWithInfo = _context.Medecins.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque médecin
            var medecinList = medecinsWithInfo.Select(m => new { Id = m.Id, FullName = $"{m.Nom} {m.Prenom}" }).ToList();
            ViewData["Patients"] = new SelectList(patientList, "Id", "FullName",rendezVous.PatientId);
            ViewData["Medecins"] = new SelectList(medecinList, "Id", "FullName", rendezVous.MedecinId);
            return View(rendezVous);
        }

        // GET: RendezVous/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RendezVous == null)
            {
                return NotFound();
            }

            var rendezVous = await _context.RendezVous
                .Include(r => r.Medecin)
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rendezVous == null)
            {
                return NotFound();
            }

            return View(rendezVous);
        }

        // POST: RendezVous/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RendezVous == null)
            {
                return Problem("Entity set 'PatientDbContext.RendezVous'  is null.");
            }
            var rendezVous = await _context.RendezVous.FindAsync(id);
            if (rendezVous != null)
            {
                _context.RendezVous.Remove(rendezVous);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RendezVousExists(int id)
        {
          return (_context.RendezVous?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public IActionResult SearchByDate(DateTime selectedDate)
        {
            // Récupérer les rendez-vous pour la date sélectionnée
            var rendezVousList = _context.RendezVous
                .Where(r => r.DateRendezVous.Date == selectedDate.Date)
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .ToList();

            // Passez les rendez-vous à la vue
            return View(rendezVousList);
        }
    }
}
