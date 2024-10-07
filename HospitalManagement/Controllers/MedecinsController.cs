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
    public class MedecinsController : Controller
    {
        private readonly PatientDbContext _context;

        public MedecinsController(PatientDbContext context)
        {
            _context = context;
        }

        // GET: Medecins
        public async Task<IActionResult> Index()
        {
              return _context.Medecins != null ? 
                          View(await _context.Medecins.ToListAsync()) :
                          Problem("Entity set 'PatientDbContext.Medecins'  is null.");
        }

        // GET: Medecins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medecins == null)
            {
                return NotFound();
            }

            var medecin = await _context.Medecins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medecin == null)
            {
                return NotFound();
            }

            return View(medecin);
        }

        // GET: Medecins/Create
        public IActionResult Create()
        {
            // Create a list of genres and store it in ViewBag
            var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString()
            }).ToList();

            ViewBag.Genres = genres;
            return View();
        }

        // POST: Medecins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Specialite,DateEmbauche,Adresse,NumeroTelephone,Email,Genre")] Medecin medecin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medecin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medecin);
        }

        // GET: Medecins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medecins == null)
            {
                return NotFound();
            }

            var medecin = await _context.Medecins.FindAsync(id);
            if (medecin == null)
            {
                return NotFound();
            }

            // Create a list of genres and store it in ViewBag
            var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == medecin.Genre
            }).ToList();

            ViewBag.Genres = genres;

            return View(medecin);
        }

        // POST: Medecins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Specialite,DateEmbauche,Adresse,NumeroTelephone,Email,Genre")] Medecin medecin)
        {
            if (id != medecin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medecin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedecinExists(medecin.Id))
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

            // If ModelState is not valid, repopulate the list of genres in ViewBag and return the view
            var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == medecin.Genre
            }).ToList();

            ViewBag.Genres = genres;

            return View(medecin);
        }

        // GET: Medecins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medecins == null)
            {
                return NotFound();
            }

            var medecin = await _context.Medecins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medecin == null)
            {
                return NotFound();
            }

            return View(medecin);
        }

        // POST: Medecins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medecins == null)
            {
                return Problem("Entity set 'PatientDbContext.Medecins'  is null.");
            }
            var medecin = await _context.Medecins.FindAsync(id);
            if (medecin != null)
            {
                _context.Medecins.Remove(medecin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedecinExists(int id)
        {
          return (_context.Medecins?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        public IActionResult MesPatients(int id)
        {
            // Récupérer le médecin avec les dossiers médicaux associés aux patients
            var medecin = _context.Medecins
                .Include(m => m.DossiersMedicaux)
                    .ThenInclude(dm => dm.Patient)
                .FirstOrDefault(m => m.Id == id);

            if (medecin == null)
            {
                return NotFound();
            }

            // Récupérer la liste des patients associés au médecin
            var patients = medecin.DossiersMedicaux
                .Select(dm => dm.Patient)
                .ToList();

            // Vous pouvez maintenant passer la liste de patients à la vue
            return View(patients);
        }

        // Action pour la recherche d'un medecin par Adresse
        public IActionResult SearchbyAdresse(string adresse)
        {
            var medecins = _context.Medecins.ToList();
            if (String.IsNullOrEmpty(adresse)) return View(medecins);
            var s = _context.Medecins.Where(x => x.Adresse.Contains(adresse)).ToList();
            return View(s.ToList()); ;
        }


        // Action pour la recherche d'un medecin par specialite
        public IActionResult SearchbySpeciality(string speciality)
        {
            var medecins = _context.Medecins.ToList();
            if (String.IsNullOrEmpty(speciality)) return View(medecins);
            var s = _context.Medecins.Where(x => x.Specialite.Contains(speciality)).ToList();
            return View(s.ToList()); ;
        }


        public IActionResult SearchBy2(string speciality, string adresse)
        {
            var medecins = _context.Medecins.AsQueryable();
            ViewBag.Adresses = medecins.Select(m => m.Adresse).Distinct().ToList();
            if (adresse != "All")
            {
                medecins = medecins.Where(x => x.Adresse == adresse);
            }
            if (!String.IsNullOrEmpty(speciality))
            {
                medecins = medecins.Where(x => x.Specialite.Contains(speciality));
            }
            return View(medecins.ToList());
        }

    }
}
