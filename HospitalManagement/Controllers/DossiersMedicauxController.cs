using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Models.HospitalModel;

namespace HospitalManagement.Controllers
{
    public class DossiersMedicauxController : Controller
    {
        private readonly PatientDbContext _context;

        public DossiersMedicauxController(PatientDbContext context)
        {
            _context = context;
        }

        // GET: DossiersMedicaux
        public async Task<IActionResult> Index()
        {
            var patientDbContext = _context.DossiersMedicaux.Include(d => d.Medecin).Include(d => d.Patient).Include(d => d.RendezVous);
            return View(await patientDbContext.ToListAsync());
        }

        // GET: DossiersMedicaux/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DossiersMedicaux == null)
            {
                return NotFound();
            }

            var dossiersMedicaux = await _context.DossiersMedicaux
                .Include(d => d.Medecin)
                .Include(d => d.Patient)
                .Include(d => d.RendezVous)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dossiersMedicaux == null)
            {
                return NotFound();
            }

            return View(dossiersMedicaux);
        }

        // GET: DossiersMedicaux/Create
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
            ViewData["RendezVousId"] = new SelectList(_context.RendezVous, "Id", "Id");

            return View();
        }

        // POST: DossiersMedicaux/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Diagnostic,PatientId,MedecinId,RendezVousId")] DossiersMedicaux dossiersMedicaux)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dossiersMedicaux);
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
            ViewData["RendezVousId"] = new SelectList(_context.RendezVous, "Id", "Id", dossiersMedicaux.RendezVousId);

            return View(dossiersMedicaux);
        }


        // GET: DossiersMedicaux/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DossiersMedicaux == null)
            {
                return NotFound();
            }

            var dossiersMedicaux = await _context.DossiersMedicaux.FindAsync(id);
            if (dossiersMedicaux == null)
            {
                return NotFound();
            }
            // Sélectionnez les patients avec leurs informations
            var patientsWithInfo = _context.Patients.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque patient
            var patientList = patientsWithInfo.Select(p => new { Id = p.Id, FullName = $"{p.Nom} {p.Prenom}" }).ToList();

            // Sélectionnez les médecins avec leurs informations
            var medecinsWithInfo = _context.Medecins.ToList();

            // Créez une liste avec des objets anonymes contenant Id, Nom et Prenom pour chaque médecin
            var medecinList = medecinsWithInfo.Select(m => new { Id = m.Id, FullName = $"{m.Nom} {m.Prenom}" }).ToList();

            // Passez la liste à la vue via ViewBag
            ViewData["Patients"] = new SelectList(patientList, "Id", "FullName", dossiersMedicaux.PatientId);
            ViewData["Medecins"] = new SelectList(medecinList, "Id", "FullName", dossiersMedicaux.MedecinId);
            ViewData["RendezVousId"] = new SelectList(_context.RendezVous, "Id", "Id", dossiersMedicaux.RendezVousId);

            return View(dossiersMedicaux);
        }

        // POST: DossiersMedicaux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Diagnostic,PatientId,MedecinId,RendezVousId")] DossiersMedicaux dossiersMedicaux)
        {
            if (id != dossiersMedicaux.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dossiersMedicaux);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DossiersMedicauxExists(dossiersMedicaux.Id))
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
            ViewData["Patients"] = new SelectList(patientList, "Id", "FullName", dossiersMedicaux.PatientId);
            ViewData["Medecins"] = new SelectList(medecinList, "Id", "FullName", dossiersMedicaux.MedecinId);
            ViewData["RendezVousId"] = new SelectList(_context.RendezVous, "Id", "Id", dossiersMedicaux.RendezVousId);
            return View(dossiersMedicaux);
        }

        // GET: DossiersMedicaux/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DossiersMedicaux == null)
            {
                return NotFound();
            }

            var dossiersMedicaux = await _context.DossiersMedicaux
                .Include(d => d.Medecin)
                .Include(d => d.Patient)
                .Include(d => d.RendezVous)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dossiersMedicaux == null)
            {
                return NotFound();
            }

            return View(dossiersMedicaux);
        }

        // POST: DossiersMedicaux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DossiersMedicaux == null)
            {
                return Problem("Entity set 'PatientDbContext.DossiersMedicaux'  is null.");
            }
            var dossiersMedicaux = await _context.DossiersMedicaux.FindAsync(id);
            if (dossiersMedicaux != null)
            {
                _context.DossiersMedicaux.Remove(dossiersMedicaux);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DossiersMedicauxExists(int id)
        {
          return (_context.DossiersMedicaux?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IActionResult SearchByPatient(string nom, string prenom)
        {
            // Récupérer les dossiers médicaux pour le patient avec le nom et le prénom spécifiés
            var dossiersMedicauxList = _context.DossiersMedicaux
                .Where(dm => dm.Patient.Nom == nom && dm.Patient.Prenom == prenom)
                .Include(dm => dm.Patient)
                .Include(dm => dm.Medecin)
                .Include(dm => dm.RendezVous)
                .ToList();

            // Passez les dossiers médicaux à la vue
            return View(dossiersMedicauxList);
        }

        public IActionResult SearchByDoctor(string nom, string prenom)
        {
            // Récupérer les dossiers médicaux pour le médecin avec le nom et le prénom spécifiés
            var dossiersMedicauxList = _context.DossiersMedicaux
                .Where(dm => dm.Medecin.Nom == nom && dm.Medecin.Prenom == prenom)
                .Include(dm => dm.Patient)
                .Include(dm => dm.Medecin)
                .Include(dm => dm.RendezVous)
                .ToList();

            // Passez les dossiers médicaux à la vue
            return View("SearchByDoctor", dossiersMedicauxList);
        }


    }
}
