using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models.HospitalModel
{
    public class Patient
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le champ Nom est obligatoire")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Le champ Prenom est obligatoire")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "Le champ date de Naissance est obligatoire")]
        public DateTime DateNaissance { get; set; }
        [Required(ErrorMessage = "Le champ Adresse est obligatoire")]
        public string Adresse { get; set; }
     
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Le champ Email n'a pas un format valide.")]
        public string Email { get; set; }

        public string NumeroTelephone { get; set; }

        public Genre? Genre { get; set; }
        // Ajouter une propriété de navigation pour les rendez-vous associés au patient
        public virtual ICollection<RendezVous>? RendezVous { get; set; } = new List<RendezVous>();
        // Ajouter une propriété de navigation pour les dossiers médicaux associés au patient
        public virtual ICollection<DossiersMedicaux>? DossiersMedicaux { get; set; } = new List<DossiersMedicaux>();
    }
   
}
