using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class dossiersmedicaux : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DossiersMedicaux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnostic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    MedecinId = table.Column<int>(type: "int", nullable: false),
                    RendezVousId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DossiersMedicaux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DossiersMedicaux_Medecins_MedecinId",
                        column: x => x.MedecinId,
                        principalTable: "Medecins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DossiersMedicaux_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DossiersMedicaux_RendezVous_RendezVousId",
                        column: x => x.RendezVousId,
                        principalTable: "RendezVous",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DossiersMedicaux_MedecinId",
                table: "DossiersMedicaux",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_DossiersMedicaux_PatientId",
                table: "DossiersMedicaux",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DossiersMedicaux_RendezVousId",
                table: "DossiersMedicaux",
                column: "RendezVousId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DossiersMedicaux");
        }
    }
}
