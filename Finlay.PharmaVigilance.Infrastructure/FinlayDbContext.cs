

using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure;


public class FinlayDbContext : IdentityDbContext<User, Role, Guid>
{
        public FinlayDbContext(DbContextOptions options) : base(options)
        {

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
                var entriesGeneric = ChangeTracker.Entries<BasicEntity>();

                foreach (var entry in entriesGeneric)
                {
                        if (entry.State == EntityState.Added)
                        {
                                entry.Entity.CreatedAt = TimeZoneHelper.GetEasternNow();
                                entry.Entity.UpdatedAt = TimeZoneHelper.GetEasternNow();
                        }

                        if (entry.State == EntityState.Modified)
                        {
                                entry.Entity.UpdatedAt = TimeZoneHelper.GetEasternNow();
                        }
                }

                var entriesUser = ChangeTracker.Entries<User>();

                foreach (var entry in entriesUser)
                {
                        if (entry.State == EntityState.Added)
                        {
                                entry.Entity.CreatedAt = DateTime.UtcNow;
                                entry.Entity.UpdatedAt = DateTime.UtcNow;
                        }

                        if (entry.State == EntityState.Modified)
                        {
                                entry.Entity.UpdatedAt = DateTime.UtcNow;
                        }
                }

                return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdverseEvent> AdverseEvents { get; set; }
        // public DbSet<AdverseEventSymptom> AdverseEventSymptoms { get; set; }
        public DbSet<AefiReport> AefiReport { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<MedicalReview> MedicalReviews { get; set; }
        public DbSet<MedicalReviewer> MedicalReviewers { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Reporter> Reporters { get; set; }
        public DbSet<SectionResponsible> SectionResponsibles { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<VaccinatedSubject> VaccinatedSubjects { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<MedicalReviewAssignment> MedicalReviewAssignments { get; set; }
        public DbSet<VaccinationCenter> VaccinationCenters { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
                base.OnModelCreating(builder);

                builder.Entity<Province>().HasData(
                            new Province { Id = 1, Name = "Pinar del Rio" },
                            new Province { Id = 2, Name = "Artemisa" },
                            new Province { Id = 3, Name = "Mayabeque" },
                            new Province { Id = 4, Name = "Isla de la Juventud" },
                            new Province { Id = 5, Name = "La Habana" },
                            new Province { Id = 6, Name = "Matanzas" },
                            new Province { Id = 7, Name = "Cienfuegos" },
                            new Province { Id = 8, Name = "Villa Clara" },
                            new Province { Id = 9, Name = "Sancti Spiritus" },
                            new Province { Id = 10, Name = "Ciego de Avila" },
                            new Province { Id = 11, Name = "Camaguey" },
                            new Province { Id = 12, Name = "Las Tunas" },
                            new Province { Id = 13, Name = "Granma" },
                            new Province { Id = 14, Name = "Holguin" },
                            new Province { Id = 15, Name = "Santiago de Cuba" },
                            new Province { Id = 16, Name = "Guantanamo" }
                        );

                builder.Entity<Municipality>().HasData(
                    // Pinar del Río
                    new Municipality { Id = 1, Name = "Pinar del Río", ProvinceId = 1 },
                    new Municipality { Id = 2, Name = "Viñales", ProvinceId = 1 },

                    // Artemisa
                    new Municipality { Id = 3, Name = "Artemisa", ProvinceId = 2 },
                    new Municipality { Id = 4, Name = "Mariel", ProvinceId = 2 },

                    // Mayabeque
                    new Municipality { Id = 5, Name = "San José de las Lajas", ProvinceId = 3 },
                    new Municipality { Id = 6, Name = "Güines", ProvinceId = 3 },

                    // Isla de la Juventud
                    new Municipality { Id = 7, Name = "Nueva Gerona", ProvinceId = 4 },
                    new Municipality { Id = 8, Name = "Isla de la Juventud rural", ProvinceId = 4 },

                    // La Habana
                    new Municipality { Id = 9, Name = "Plaza de la Revolución", ProvinceId = 5 },
                    new Municipality { Id = 10, Name = "Playa", ProvinceId = 5 },

                    // Matanzas
                    new Municipality { Id = 11, Name = "Matanzas", ProvinceId = 6 },
                    new Municipality { Id = 12, Name = "Varadero", ProvinceId = 6 },

                    // Cienfuegos
                    new Municipality { Id = 13, Name = "Cienfuegos", ProvinceId = 7 },
                    new Municipality { Id = 14, Name = "Cruces", ProvinceId = 7 },

                    // Villa Clara
                    new Municipality { Id = 15, Name = "Santa Clara", ProvinceId = 8 },
                    new Municipality { Id = 16, Name = "Caibarién", ProvinceId = 8 },

                    // Sancti Spíritus
                    new Municipality { Id = 17, Name = "Sancti Spíritus", ProvinceId = 9 },
                    new Municipality { Id = 18, Name = "Trinidad", ProvinceId = 9 },

                    // Ciego de Ávila
                    new Municipality { Id = 19, Name = "Ciego de Ávila", ProvinceId = 10 },
                    new Municipality { Id = 20, Name = "Morón", ProvinceId = 10 },

                    // Camagüey
                    new Municipality { Id = 21, Name = "Camagüey", ProvinceId = 11 },
                    new Municipality { Id = 22, Name = "Florida", ProvinceId = 11 },

                    // Las Tunas
                    new Municipality { Id = 23, Name = "Las Tunas", ProvinceId = 12 },
                    new Municipality { Id = 24, Name = "Puerto Padre", ProvinceId = 12 },

                    // Granma
                    new Municipality { Id = 25, Name = "Bayamo", ProvinceId = 13 },
                    new Municipality { Id = 26, Name = "Manzanillo", ProvinceId = 13 },

                    // Holguín
                    new Municipality { Id = 27, Name = "Holguín", ProvinceId = 14 },
                    new Municipality { Id = 28, Name = "Banes", ProvinceId = 14 },

                    // Santiago de Cuba
                    new Municipality { Id = 29, Name = "Santiago de Cuba", ProvinceId = 15 },
                    new Municipality { Id = 30, Name = "Contramaestre", ProvinceId = 15 },

                    // Guantánamo
                    new Municipality { Id = 31, Name = "Guantánamo", ProvinceId = 16 },
                    new Municipality { Id = 32, Name = "Baracoa", ProvinceId = 16 }
                );

                builder.Entity<User>(entity =>
                {
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.UserRole)
                      .IsRequired();
                        entity.Property(e => e.CreatedAt)
                      .IsRequired();
                        entity.Property(e => e.UpdatedAt)
                      .IsRequired();
                        // entity.Property(e=> e.RefreshToken)
                        //       .IsRequired();
                });


                builder.Entity<Role>(entity =>
                {
                        entity.HasKey(e => e.Id);

                });

                builder.Entity<MedicalReviewer>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.HasOne(mr => mr.User)
                  .WithOne()
                  .HasForeignKey<MedicalReviewer>(mr => mr.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(mr => mr.SectionResponsible)
                    .WithMany(sr => sr.MedicalReviewers)
                    .HasForeignKey(mr => mr.SectionResponsibleId)
                    .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(mr => mr.Province)
                  .WithMany()
                  .HasForeignKey(mr => mr.ProvinceId)
                  .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(mr => mr.Municipality)
                  .WithMany()
                  .HasForeignKey(mr => mr.MunicipalityId)
                  .OnDelete(DeleteBehavior.Restrict);

                        entity.Property(e => e.Institution)
                     .IsRequired()
                     .HasMaxLength(100);

                        entity.Property(e => e.ProfessionalLicense)
                          .IsRequired()
                          .HasMaxLength(20);

                        entity.Property(e => e.Specialty)
                          .IsRequired(false)
                          .HasMaxLength(100);

                        entity.Property(e => e.IdentityNumber)
                        .IsRequired()
                        .HasMaxLength(20);

                        entity.HasIndex(e => e.IdentityNumber)
                .IsUnique();



                });

                builder.Entity<SectionResponsible>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.HasOne(sr => sr.User)
                  .WithOne()
                  .HasForeignKey<SectionResponsible>(sr => sr.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(sr => sr.Province)
                    .WithMany()
                    .HasForeignKey(sr => sr.ProvinceId)
                    .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(sr => sr.Municipality)
                  .WithMany()
                  .HasForeignKey(sr => sr.MunicipalityId)
                  .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(sr => sr.Admin)
                    .WithMany(ad => ad.RegisteredResponsibles)
                    .HasForeignKey(sr => sr.AdminId)
                    .OnDelete(DeleteBehavior.Restrict);

                });


                // Report DbContext configurations for relationships and constraints
                builder.Entity<AdverseEvent>(entity =>
                {
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.StartDate)
                      .IsRequired();
                        entity.Property(e => e.Description)
                      .IsRequired(false)
                      .HasMaxLength(800);
                        entity.Property(e => e.CurrentStatus)
                      .IsRequired()
                      .HasConversion<string>();
                        entity.Property(e => e.Intensity)
                      .IsRequired()
                      .HasConversion<string>();
                        entity.Property(e => e.SeverityLevel)
                      .IsRequired()
                      .HasConversion<string>();
                        entity.Property(e => e.VisitedDoctor)
                    .IsRequired();

                        entity.Property(e => e.WentToEmergencyRoom)
                    .IsRequired();

                        entity.Property(e => e.PermanentDisability)
                    .IsRequired();

                        entity.Property(e => e.IsLifeThreatening)
                    .IsRequired();

                        entity.Property(e => e.ResultedInDeath)
                    .IsRequired();

                        entity.Property(e => e.LaboratoryResults)
                    .IsRequired(false)
                    .HasMaxLength(300);

                        entity.Property(e => e.MedDRACode)
                    .IsRequired(false)
                    .HasMaxLength(200);

                        entity.Property(e => e.RetClassification)
                    .IsRequired(false)
                    .HasMaxLength(200);



                        entity.HasOne(e => e.AefiReport)
                  .WithMany(r => r.AdverseEvents)
                  .HasForeignKey(e => e.AefiReportId)
                  .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(e => e.Symptom)
                  .WithMany(s => s.AdverseEvents)
                  .HasForeignKey(e => e.SymptomId)
                  .OnDelete(DeleteBehavior.Restrict);

                });

                builder.Entity<AefiReport>(entity =>
                {
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.ReportDate)
                      .IsRequired();

                        entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(50);

                        entity.Property(e => e.NotificationNumber)
                      .IsRequired()
                      .HasMaxLength(100);

                        entity.HasIndex(e => e.NotificationNumber)
                .IsUnique();

                        entity.Property(e => e.isMedicalReport)
                    .IsRequired();

                        entity.HasOne(r => r.VaccinatedSubject)
                  .WithMany(p => p.AefiReports)
                  .HasForeignKey(r => r.VaccinatedSubjectId)
                  .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(r => r.Reporter)
                .WithMany(p => p.AefiReports)
                .HasForeignKey(r => r.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

                });

                builder.Entity<Symptom>(entity =>
                {
                        entity.HasKey(p => p.Id);

                        entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(120);

                        entity.Property(p => p.NormalizedName)
                .IsRequired(false)
                .HasMaxLength(120);

                        entity.HasIndex(p => p.NormalizedName)
                .IsUnique()
                .HasFilter("`NormalizedName` IS NOT NULL");

                        entity.Property(e => e.Description)
                        .IsRequired(false)
                        .HasMaxLength(300);


                        entity.Property(p => p.CodingSystem)
                    .IsRequired()
                    .HasMaxLength(30);

                        entity.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(30);

                        entity.Property(p => p.StandardCode)
                .IsRequired()
                .HasMaxLength(30);

                        entity.Property(p => p.IsActive)
                    .IsRequired();

                });

                builder.Entity<Vaccination>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.Site)
                  .IsRequired()
                  .HasConversion<string>();

                        entity.Property(e => e.DoseNumber)
                  .IsRequired()
                  .HasConversion<string>();

                        entity.Property(e => e.AdministrationDate)
                  .IsRequired();


                        entity.HasOne(v => v.Lot)
                  .WithMany()
                  .HasForeignKey(v => v.LotId)
                  .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(v => v.AefiReport)
                    .WithMany(ar => ar.Vaccinations)
                    .HasForeignKey(v => v.AefiReportId)
                    .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(v => v.VaccinationCenter)
                    .WithMany()
                    .HasForeignKey(v => v.VaccinationCenterId)
                    .OnDelete(DeleteBehavior.Restrict);

                });

                builder.Entity<Vaccine>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(150);

                        entity.Property(e => e.NormalizedName)
                  .IsRequired(false)
                  .HasMaxLength(150);

                        entity.HasIndex(e => e.NormalizedName)
                  .IsUnique()
                  .HasFilter("`NormalizedName` IS NOT NULL");

                        entity.Property(e => e.Type)
                  .IsRequired()
                  .HasConversion<string>();

                        entity.HasOne(e => e.Manufacturer)
                .WithMany()
                .HasForeignKey(e => e.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);


                        entity.Property(e => e.Code)
                      .IsRequired(false)
                      .HasMaxLength(30);

                        entity.Property(e => e.Description)
                  .IsRequired(false)
                  .HasMaxLength(500);

                        entity.Property(e => e.IsActive)
                  .IsRequired();
                });

                builder.Entity<VaccinatedSubject>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(150);

                        entity.Property(e => e.IdentityNumber)
                .IsRequired()
                .HasMaxLength(20);

                        entity.HasIndex(e => e.IdentityNumber)
                .IsUnique();

                        entity.Property(e => e.Gender)
                .IsRequired()
                .HasConversion<string>();

                        entity.Property(e => e.IsPregnant)
                .IsRequired(false);

                        entity.Property(e => e.HealthArea)
                .HasMaxLength(100);

                        entity.Property(e => e.Address)
                .HasMaxLength(250);

                        entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

                        entity.Property(e => e.Email)
                .HasMaxLength(100);

                        entity.Property(e => e.CurrentMedications)
                    .HasMaxLength(250);

                        entity.Property(e => e.Allergies)
                .HasMaxLength(250);

                        entity.Property(e => e.MedicalHistory)
                    .HasMaxLength(400);

                        entity.HasOne(e => e.Province)
                .WithMany()
                .HasForeignKey(e => e.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(e => e.Municipality)
                .WithMany()
                .HasForeignKey(e => e.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict);

                });

                builder.Entity<Reporter>(entity =>
               {
                       entity.HasKey(e => e.Id);

                       entity.Property(e => e.FullName)
              .IsRequired()
              .HasMaxLength(150);

                       entity.Property(p => p.IdentityNumber)
              .IsRequired()
              .HasMaxLength(20);

                       entity.HasIndex(e => e.IdentityNumber)
                .IsUnique();

                       entity.Property(e => e.ReporterRelationship)
              .IsRequired()
              .HasConversion<string>();


                       entity.Property(e => e.PhoneNumber)
              .IsRequired()
              .HasMaxLength(20);

                       entity.Property(e => e.Email)
              .IsRequired()
              .HasMaxLength(100);

                       entity.Property(e => e.ProfessionalLicense)
                   .IsRequired(false)
                   .HasMaxLength(30);

                       entity.Property(e => e.Institution)
              .IsRequired(false)
              .HasMaxLength(100);

                       entity.HasOne(e => e.Province)
              .WithMany()
              .HasForeignKey(e => e.ProvinceId)
              .OnDelete(DeleteBehavior.Restrict);

                       entity.HasOne(e => e.Municipality)
              .WithMany()
              .HasForeignKey(e => e.MunicipalityId)
              .OnDelete(DeleteBehavior.Restrict);



               });

                builder.Entity<MedicalReview>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.ClinicalSignificance)
               .IsRequired()
               .HasConversion<string>();

                        entity.Property(e => e.Causality)
               .IsRequired()
               .HasConversion<string>();

                        entity.Property(e => e.ReviewedAt)
               .IsRequired();

                        entity.HasOne(mr => mr.MedicalReviewAssignment)
                .WithOne(mra => mra.MedicalReview)
                .HasForeignKey<MedicalReview>(mr => mr.MedicalReviewAssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

                });


                builder.Entity<Admin>(entity =>
                {
                        entity.HasKey(e => e.Id);


                        entity.HasOne(sr => sr.User)
                .WithOne()
                .HasForeignKey<Admin>(sr => sr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                });


                builder.Entity<Alert>(entity =>
               {
                       entity.HasKey(e => e.Id);

                       entity.Property(a => a.ReadAt)
                   .IsRequired(false);

                       entity.Property(a => a.IsActive)
                   .IsRequired();

                       entity.Property(a => a.IsRead)
                   .IsRequired();


                       entity.HasOne(a => a.SectionResponsible)
                   .WithMany(sr => sr.ReceivedAlerts)
                   .HasForeignKey(a => a.SectionResponsibleId)
                   .OnDelete(DeleteBehavior.Restrict);


                       entity.HasOne(a => a.AefiReport)
                   .WithMany(ar => ar.Alerts)
                   .HasForeignKey(a => a.AefiReportId)
                   .OnDelete(DeleteBehavior.Restrict);

               });


                builder.Entity<MedicalReviewAssignment>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(a => a.AssignedAt)
                    .IsRequired();

                        entity.Property(a => a.Status)
                    .IsRequired()
                    .HasConversion<string>();

                        entity.Property(a => a.RejectionReason)
                    .IsRequired(false)
                    .HasMaxLength(300);


                        entity.HasOne(a => a.SectionResponsible)
                    .WithMany(sr => sr.ManagedReviews)
                    .HasForeignKey(a => a.SectionResponsibleId)
                    .OnDelete(DeleteBehavior.Restrict);


                        entity.HasOne(a => a.AefiReport)
                    .WithMany(ar => ar.MedicalReviewAssignments)
                    .HasForeignKey(a => a.AefiReportId)
                    .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(a => a.MedicalReviewer)
                   .WithMany(ar => ar.MedicalReviews)
                   .HasForeignKey(a => a.MedicalReviewerId)
                   .OnDelete(DeleteBehavior.Restrict);



                });

                builder.Entity<VaccinationCenter>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(a => a.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                        entity.Property(a => a.Address)
                   .IsRequired()
                   .HasMaxLength(300);

                        entity.HasOne(vc => vc.Municipality)
                    .WithMany()
                    .HasForeignKey(vc => vc.MunicipalityId)
                    .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(vc => vc.Province)
                    .WithMany()
                    .HasForeignKey(vc => vc.ProvinceId)
                    .OnDelete(DeleteBehavior.Restrict);

                });

                builder.Entity<Lot>(entity =>
               {
                       entity.HasKey(e => e.Id);

                       entity.Property(l => l.LotNumber)
                   .IsRequired()
                   .HasMaxLength(50);

                       entity.HasOne(l => l.Vaccine)
                   .WithMany(v => v.Lots)
                   .HasForeignKey(vc => vc.VaccineId)
                   .OnDelete(DeleteBehavior.Restrict);

               });


                builder.Entity<Manufacturer>(entity =>
               {
                       entity.HasKey(e => e.Id);

                       entity.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(150);

                       entity.HasIndex(e => e.Name);

                       entity.Property(e => e.Country)
                .IsRequired(false)
                .HasMaxLength(150);

                       //        entity.HasData(
                       //                new Manufacturer { Name = "Instituto Finlay de Vacunas (IFV)", Country = "Cuba" }
                       //        );

               });


                builder.Entity<RefreshToken>(entity =>
             {
                     entity.HasKey(e => e.Id);

                     entity.HasOne(l => l.User)
                 .WithMany()
                 .HasForeignKey(vc => vc.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

             });

        }
}