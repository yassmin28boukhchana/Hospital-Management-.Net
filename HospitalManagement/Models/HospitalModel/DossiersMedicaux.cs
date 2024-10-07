using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models.HospitalModel
{
    public class DossiersMedicaux
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le champ Diagnostic est obligatoire.")]
        public string Diagnostic { get; set; }

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; } = null!;

        [ForeignKey("MedecinId")]
        public int MedecinId { get; set; }
        public virtual Medecin? Medecin { get; set; } = null!;

        [ForeignKey("RendezVousId")]
        public int RendezVousId { get; set; }
        public virtual RendezVous? RendezVous { get; set; } = null!;

    }
}
