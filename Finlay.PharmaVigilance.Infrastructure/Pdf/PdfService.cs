// using DinkToPdf;
// using DinkToPdf.Contracts;
// using Finlay.PharmaVigilance.Application.DTO;
// using Finlay.PharmaVigilance.Application.Enum;
// using Finlay.PharmaVigilance.Application.IServices.Pdf;
// using Microsoft.AspNetCore.Hosting;
// using System.Net;
// using System.Text;

// namespace Finlay.PharmaVigilance.Infrastructure.Pdf;

// public class PdfService : IPdfService
// {
//     private readonly IConverter _converter;
//     private readonly string _templateDirectory;
//     private readonly string _logoBase64;
//     private readonly IDictionary<ReportPdfTemplateType, string> _templates;

//     public PdfService(IConverter converter, IWebHostEnvironment env)
//     {
//         _converter = converter ?? throw new ArgumentNullException(nameof(converter));
//         _templateDirectory = FindTemplateDirectory(env);

//         _templates = new Dictionary<ReportPdfTemplateType, string>
//         {
//             [ReportPdfTemplateType.Admin] = LoadTemplate("admin.html"),
//             [ReportPdfTemplateType.User] = LoadTemplate("user.html"),
//             [ReportPdfTemplateType.MedicalReview] = LoadTemplate("medical-review.html")
//         };

//         _logoBase64 = LoadLogoBase64();
//     }

//     public byte[] GenerateReportPdf(ReportPdfDto report, ReportPdfTemplateType templateType)
//     {
//         if (report == null)
//             throw new ArgumentNullException(nameof(report));

//         var template = GetTemplate(templateType);
//         var html = ReplaceMarkers(template, report);

//         return ConvertHtmlToPdf(html, report.NotificationNumber);
//     }

//     public byte[] GenerateReportPdf(ReportDetailAdminDto report, ReportPdfTemplateType templateType)
//     {
//         if (report == null)
//             throw new ArgumentNullException(nameof(report));

//         var reportDto = MapAdminReportToPdfDto(report);
//         return GenerateReportPdf(reportDto, templateType);
//     }

//     private string FindTemplateDirectory(IWebHostEnvironment env)
//     {
//         var candidates = new[]
//         {
//             Path.Combine(env.ContentRootPath, "Pdf", "Templates"),
//             Path.Combine(env.ContentRootPath, "Templates"),
//             Path.Combine(AppContext.BaseDirectory, "Pdf", "Templates"),
//             Path.Combine(AppContext.BaseDirectory, "Templates"),
//             Path.Combine(AppContext.BaseDirectory, "..", "..", "Pdf", "Templates"),
//             Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Pdf", "Templates"),
//             Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Pdf", "Templates")
//         };

//         foreach (var candidate in candidates)
//         {
//             var normalized = Path.GetFullPath(candidate);
//             if (Directory.Exists(normalized))
//                 return normalized;
//         }

//         throw new DirectoryNotFoundException("Could not locate the PDF template directory.");
//     }

//     private string LoadTemplate(string fileName)
//     {
//         var path = Path.Combine(_templateDirectory, fileName);
//         if (!File.Exists(path))
//             throw new FileNotFoundException($"PDF template not found: {path}");

//         return File.ReadAllText(path, Encoding.UTF8);
//     }

//     private string LoadLogoBase64()
//     {
//         var logoPath = Path.Combine(_templateDirectory, "logo.png");
//         if (!File.Exists(logoPath))
//             return string.Empty;

//         var logoBytes = File.ReadAllBytes(logoPath);
//         return Convert.ToBase64String(logoBytes);
//     }

//     private string GetTemplate(ReportPdfTemplateType templateType)
//     {
//         if (!_templates.TryGetValue(templateType, out var template))
//             throw new ArgumentException($"Unknown PDF template type: {templateType}", nameof(templateType));

//         return template;
//     }

//     private static string Encode(string? value)
//     {
//         return string.IsNullOrWhiteSpace(value) ? "N/A" : WebUtility.HtmlEncode(value);
//     }

//     private static string BuildVaccinationSummary(IEnumerable<VaccinationPdfDto> vaccinations)
//     {
//         if (vaccinations == null || !vaccinations.Any())
//             return "No reportado";

//         return string.Join("<br />", vaccinations.Select(v =>
//             $"{Encode(v.VaccineName)} - Lote: {Encode(v.LotNumber)} ({v.AdministrationDate:dd/MM/yyyy})"));
//     }

//     private static string BuildAdverseEventSummary(IEnumerable<AdverseEventPdfDto> events)
//     {
//         if (events == null || !events.Any())
//             return "No reportado";

//         return string.Join("<br /><br />", events.Select(a =>
//         {
//             var lines = new List<string>
//             {
//                 $"<strong>{Encode(a.SymptomName)}</strong>",
//                 Encode(a.Description),
//                 $"Inicio: {Encode(a.StartDate.ToString("dd/MM/yyyy"))}" +
//                     (a.FinishDate.HasValue ? $" - Fin: {Encode(a.FinishDate.Value.ToString("dd/MM/yyyy"))}" : string.Empty),
//                 $"Visitó médico: {(a.VisitedDoctor ? "Sí" : "No")} | Urgencias: {(a.WentToEmergencyRoom ? "Sí" : "No")} | Hospitalizado: {(a.WasHospitalized ? "Sí" : "No")}",
//                 $"Discapacidad permanente: {(a.PermanentDisability ? "Sí" : "No")} | Anomalía: {(a.Anomaly ? "Sí" : "No")} | Sin complicaciones: {(a.NoComplications ? "Sí" : "No")}",
//                 $"Estado actual: {Encode(a.CurrentStatus?.ToString())} | Intensidad: {Encode(a.Intensity?.ToString())} | Severidad: {Encode(a.SeverityLevel?.ToString())}"
//             };

//             if (a.ResultedInDeath)
//             {
//                 lines.Add($"Fallecimiento: Sí{(a.DeathDate.HasValue ? $" - Fecha: {Encode(a.DeathDate.Value.ToString("dd/MM/yyyy"))}" : string.Empty)}");
//             }

//             return string.Join("<br/>", lines);
//         }));
//     }

//     private string ReplaceMarkers(string template, ReportPdfDto report)
//     {
//         var html = template
//             .Replace("{{REPORT-DATE}}", report.ReportDate?.ToString("dd/MM/yyyy") ?? "N/A")
//             .Replace("{{REPORT-CREATION-DATE}}", report.ReportDate?.ToString("dd/MM/yyyy") ?? "N/A")
//             .Replace("{{REQUESTED-AT}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
//             .Replace("{{NOTIFICATION-NUMBER}}", Encode(report.NotificationNumber))
//             .Replace("{{STATUS}}", Encode(report.Status))
//             .Replace("{{SEVERITY}}", Encode(report.GlobalSeverityLevel))
//             .Replace("{{PATIENT-NAME}}", Encode(report.VaccinatedSubject?.FullName))
//             .Replace("{{AGE}}", Encode(report.VaccinatedSubject?.Age.ToString()))
//             .Replace("{{GENDER}}", Encode(report.VaccinatedSubject?.Gender.ToString()))
//             .Replace("{{PROVINCE}}", Encode(report.VaccinatedSubject?.ProvinceName))
//             .Replace("{{PATIENT-MUNICIPALITY}}", Encode(report.VaccinatedSubject?.MunicipalityName))
//             .Replace("{{PREGNANT}}", Encode(report.VaccinatedSubject?.IsPregnant == true ? "Sí" : "No"))
//             .Replace("{{REPORTER-NAME}}", Encode(report.Reporter?.Name))
//             .Replace("{{REPORTER-RELATIONSHIP}}", Encode(report.Reporter?.ReporterRelationship.ToString()))
//             .Replace("{{REPORTER-PROVINCE}}", Encode(report.Reporter?.ProvinceName))
//             .Replace("{{REPORTER-MUNICIPALITY}}", Encode(report.Reporter?.MunicipalityName))
//             .Replace("{{VACCINATIONS}}", BuildVaccinationSummary(report.Vaccinations))
//             .Replace("{{ADVERSE-EVENTS}}", BuildAdverseEventSummary(report.AdverseEvents))
//             .Replace("{{CAUSALITY}}", Encode(report.Causality))
//             .Replace("{{CLINICAL-SIGNIFICANCE}}", Encode(report.ClinicalSignificance))
//             .Replace("{{REVIEWED-AT}}", Encode(report.ReviewedAt?.ToString("dd/MM/yyyy")))
//             .Replace("{{LOGO-BASE64}}", _logoBase64);

//         if (!string.IsNullOrEmpty(_logoBase64))
//             html = html.Replace("src=\"logo.png\"", $"src=\"data:image/png;base64,{_logoBase64}\"");

//         return html;
//     }

//     private byte[] ConvertHtmlToPdf(string html, string notificationNumber)
//     {
//         var doc = new HtmlToPdfDocument()
//         {
//             GlobalSettings = {
//                 ColorMode = ColorMode.Color,
//                 Orientation = Orientation.Portrait,
//                 PaperSize = PaperKind.A4,
//                 Margins = new MarginSettings {
//                     Top = 15,
//                     Bottom = 25,  // Más espacio abajo para el footer
//                     Left = 15,
//                     Right = 15
//                 },
//                 DocumentTitle = $"Reporte FV - {notificationNumber}"
//             },
//             Objects = {
//                 new ObjectSettings
//                 {
//                     HtmlContent = html,
//                     WebSettings = {
//                         DefaultEncoding = "utf-8",
//                         EnableIntelligentShrinking = true,
//                         LoadImages = true,
//                         PrintMediaType = true
//                     },
//                     // Si necesitas header en cada página
//                     HeaderSettings = new HeaderSettings
//                     {
//                         FontSize = 8,
//                         Right = "Generado: [date] [time]",
//                         Line = false,
//                         Spacing = 3
//                     },
//                     FooterSettings = new FooterSettings
//                     {
//                         FontSize = 8,
//                         Left = "CONFIDENCIAL - Instituto Finlay de Vacunas",
//                         Right = "Página [page] de [toPage]",
//                         Line = true,
//                         Spacing = 5
//                     }
//                 }
//             }
//         };

//         return _converter.Convert(doc);
//     }

//     private static ReportPdfDto MapAdminReportToPdfDto(ReportDetailAdminDto report)
//     {
//         return new ReportPdfDto
//         {
//             ReportDate = report.ReportDate,
//             NotificationNumber = report.NotificationNumber,
//             Status = report.Status.ToString(),
//             GlobalSeverityLevel = report.GlobalSeverityLevel.ToString(),
//             VaccinatedSubject = new VaccinatedSubjectPdfDto
//             {
//                 FullName = string.Empty,
//                 Age = report.VaccinatedSubject.Age,
//                 Gender = report.VaccinatedSubject.Gender,
//                 ProvinceName = report.VaccinatedSubject.ProvinceName,
//                 MunicipalityName = report.VaccinatedSubject.MunicipalityName,
//                 IsPregnant = report.VaccinatedSubject.IsPregnant ?? false
//             },
//             Vaccinations = report.Vaccinations.Select(v => new VaccinationPdfDto
//             {
//                 VaccineName = v.VaccineName,
//                 LotNumber = v.LotNumber,
//                 AdministrationDate = v.AdministrationDate
//             }).ToList(),
//             AdverseEvents = report.AdverseEvents.Select(a => new AdverseEventPdfDto
//             {
//                 StartDate = a.StartDate,
//                 FinishDate = null,
//                 Description = a.Description,
//                 VisitedDoctor = a.VisitedDoctor,
//                 WentToEmergencyRoom = a.WentToEmergencyRoom,
//                 PermanentDisability = a.PermanentDisability,
//                 WasHospitalized = false,
//                 Anomaly = false,
//                 NoComplications = false,
//                 ResultedInDeath = a.ResultedInDeath,
//                 DeathDate = a.DeathDate,
//                 CurrentStatus = a.CurrentStatus,
//                 Intensity = a.Intensity,
//                 SeverityLevel = a.SeverityLevel,
//                 SymptomName = a.Symptom
//             }).ToList(),
//             Reporter = new ReporterPdfDto
//             {
//                 Name = report.Reporter.FullName,
//                 ReporterRelationship = report.Reporter.reporterRelationship,
//                 ProvinceName = string.Empty,
//                 MunicipalityName = string.Empty
//             },
//             Causality = report.MedicalReview != null ? report.MedicalReview.Causality.ToString() : null,
//             ClinicalSignificance = report.MedicalReview != null ? report.MedicalReview.ClinicalSignificance.ToString() : null,
//             ReviewedAt = report.MedicalReview?.ReviewedAt
//         };
//     }
// }