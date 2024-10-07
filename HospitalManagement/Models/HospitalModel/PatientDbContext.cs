using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HospitalManagement.Models.HospitalModel
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions options) :base(options)
        {

        }
        public DbSet<Patient> Patients { get; set; } = null! ;
        public DbSet<Medecin> Medecins { get; set; } = null!;
        public DbSet<RendezVous> RendezVous { get; set; } = null!;
        public DbSet<DossiersMedicaux> DossiersMedicaux { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DossiersMedicaux>()
                .HasOne(dm => dm.Patient)
                .WithMany(p => p.DossiersMedicaux)
                .HasForeignKey(dm => dm.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DossiersMedicaux>()
                .HasOne(dm => dm.Medecin)
                .WithMany(m => m.DossiersMedicaux)
                .HasForeignKey(dm => dm.MedecinId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DossiersMedicaux>()
                .HasOne(dm => dm.RendezVous)
                .WithMany(rv => rv.DossiersMedicaux)
                .HasForeignKey(dm => dm.RendezVousId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

    }  
}
