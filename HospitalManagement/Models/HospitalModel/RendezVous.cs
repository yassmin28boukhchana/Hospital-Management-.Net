using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.HospitalModel
{
    public class RendezVous
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le champ DateRendezVous est obligatoire.")]
        public DateTime DateRendezVous { get; set; }

        [Required(ErrorMessage = "Le champ Motif est obligatoire.")]
        public string Motif { get; set; }

        // Clé étrangère pour le patient
        [ForeignKey("PatientID")]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; } = null!;

        // Clé étrangère pour le médecin
        [ForeignKey("MedecinID")]
        public int MedecinId { get; set; }
        public virtual Medecin? Medecin { get; set; } = null!;
        // Ajouter une propriété de navigation pour les dossiers médicaux associés au rendezvous
        public virtual ICollection<DossiersMedicaux>? DossiersMedicaux { get; set; } = new List<DossiersMedicaux>();

        internal object OrderByDescending(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
