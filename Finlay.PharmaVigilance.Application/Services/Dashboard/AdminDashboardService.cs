using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;


public class AdminDashboardService : IAdminDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdminDashboardService> _logger;

    public AdminDashboardService(
        IUnitOfWork unitOfWork,
        ILogger<AdminDashboardService> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }


    public async Task<AdminReportDashboardDto> GetReportAsync()
    {
        return new AdminReportDashboardDto
        {
            TotalReports = 900,
            Submitted = 150,
            UnderReview = 120,
            Approved = 400,
            Rejected = 50,
            Closed = 180,

            MonthlyTrends = new List<MonthlyReportTrendDto>
        {
            new MonthlyReportTrendDto
            {
                Year = 2025,
                Month = 6,
                TotalReports = 52,

            },

            new MonthlyReportTrendDto
            {
                Year = 2025,
                Month = 7,
                TotalReports = 61,

            },

            new MonthlyReportTrendDto
            {
                Year = 2025,
                Month = 8,
                TotalReports = 74,

            },

            new MonthlyReportTrendDto
            {
                Year = 2025,
                Month = 9,
                TotalReports = 68,

            },

            new MonthlyReportTrendDto
            {
                Year = 2025,
                Month = 10,
                TotalReports = 79,

            },

            new MonthlyReportTrendDto
            {
                Year = 2025,
                Month = 11,
                TotalReports = 83,

            },

            new MonthlyReportTrendDto
            {
                Year = 2025,
                Month = 12,
                TotalReports = 91,

            },

            new MonthlyReportTrendDto
            {
                Year = 2026,
                Month = 1,
                TotalReports = 87,

            },

            new MonthlyReportTrendDto
            {
                Year = 2026,
                Month = 2,
                TotalReports = 76,

            },

            new MonthlyReportTrendDto
            {
                Year = 2026,
                Month = 3,
                TotalReports = 71,

            },

            new MonthlyReportTrendDto
            {
                Year = 2026,
                Month = 4,
                TotalReports = 85,

            },

            new MonthlyReportTrendDto
            {
                Year = 2026,
                Month = 5,
                TotalReports = 73,

            }
        },


            Provinces = new List<ProvinceReportStatusDto>
        {
            new ProvinceReportStatusDto
            {
                ProvinceName = "La Habana",
                Submitted = 42,
                UnderReview = 18,
                Approved = 95,
                Rejected = 12,
                Closed = 140,
                Serious = 28,
                Total = 307
            },

            new ProvinceReportStatusDto
            {
                ProvinceName = "Santiago de Cuba",
                Submitted = 25,
                UnderReview = 11,
                Approved = 72,
                Rejected = 8,
                Closed = 96,
                Serious = 19,
                Total = 212
            },

            new ProvinceReportStatusDto
            {
                ProvinceName = "Villa Clara",
                Submitted = 19,
                UnderReview = 9,
                Approved = 54,
                Rejected = 5,
                Closed = 77,
                Serious = 14,
                Total = 164
            },

            new ProvinceReportStatusDto
            {
                ProvinceName = "Camagüey",
                Submitted = 16,
                UnderReview = 7,
                Approved = 43,
                Rejected = 4,
                Closed = 61,
                Serious = 10,
                Total = 131
            },

            new ProvinceReportStatusDto
            {
                ProvinceName = "Holguín",
                Submitted = 21,
                UnderReview = 13,
                Approved = 58,
                Rejected = 6,
                Closed = 84,
                Serious = 16,
                Total = 182
            }
        },

            SeverityDistribution = new List<SeverityLevelDistributionDto>
        {
            new SeverityLevelDistributionDto
            {
                SeverityType = "No serio",
                Count = 45,
                Percentage = 25
            },

            new SeverityLevelDistributionDto
            {
                SeverityType = "VisitedDoctor",
                Count = 78,
                Percentage = 22.4
            },

            new SeverityLevelDistributionDto
            {
                SeverityType = "WentToEmergencyRoom",
                Count = 24,
                Percentage = 8.1
            },

            new SeverityLevelDistributionDto
            {
                SeverityType = "PermanentDisability",
                Count = 11,
                Percentage = 6.0
            },

            new SeverityLevelDistributionDto
            {
                SeverityType = "IsLifeThreatening",
                Count = 5,
                Percentage = 2.7
            },

            new SeverityLevelDistributionDto
            {
                SeverityType = "ResultedInDeath",
                Count = 66,
                Percentage = 35.8
            }
        },

            CausalityDistribution = new List<CausalityDistributionDto>
{
    new CausalityDistributionDto
    {
        Causality = "Definitive",
        Count = 74,
        Percentage = 29.5
    },

    new CausalityDistributionDto
    {
        Causality = "Probable",
        Count = 89,
        Percentage = 35.5
    },

    new CausalityDistributionDto
    {
        Causality = "Possible",
        Count = 56,
        Percentage = 22.3
    },

    new CausalityDistributionDto
    {
        Causality = "Improbable",
        Count = 21,
        Percentage = 8.4
    },

    new CausalityDistributionDto
    {
        Causality = "NotEvaluable",
        Count = 11,
        Percentage = 4.3
    }
},

            SignificanceDistribution = new List<SignificanceDistributionDto>
{
    new SignificanceDistributionDto
    {
        Significance = "ClinicallySignificantAndUnexpected",
        Count = 82,
        Percentage = 22.2
    },

    new SignificanceDistributionDto
    {
        Significance = "ExpectedEvent",
        Count = 173,
        Percentage = 46.9
    },

    new SignificanceDistributionDto
    {
        Significance = "SeriousOrLifeThreatening",
        Count = 71,
        Percentage = 19.2
    },

    new SignificanceDistributionDto
    {
        Significance = "MinorEvent",
        Count = 43,
        Percentage = 11.7
    }
},

        };
    }


    public async Task<AdminPerformanceDashboardDto> GetPerformanceAsync()
    {
        return new AdminPerformanceDashboardDto
        {
            ActiveDoctors = 128,
            AvgReportsPerDoctor = 14.7,
            AvgReviewTimeHours = 26.4,
            AvgAssignmentHours = 5.8,

            ActiveMedicalReviewers = new List<ProvinceMedicalActivityDto>
        {
            new ProvinceMedicalActivityDto
            {
                ProvinceName = "La Habana",
                ActiveDoctors = 34,
                AvgReportsPerDoctor = 18.2,
                AvgReviewTimeHours = 22.5,
                AvgAssignmentHours = 4.1,

                Municipalities = new List<MunicipalityMedicalActivityDto>
                {
                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Plaza de la Revolución",
                        ActiveDoctors = 8,
                        AvgReportsPerDoctor = 21.3,
                        AvgReviewTimeHours = 18.4,
                        AvgAssignmentHours = 3.2
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Centro Habana",
                        ActiveDoctors = 6,
                        AvgReportsPerDoctor = 19.5,
                        AvgReviewTimeHours = 21.7,
                        AvgAssignmentHours = 4.0
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Habana del Este",
                        ActiveDoctors = 7,
                        AvgReportsPerDoctor = 16.8,
                        AvgReviewTimeHours = 24.9,
                        AvgAssignmentHours = 4.5
                    }
                }
            },

            new ProvinceMedicalActivityDto
            {
                ProvinceName = "Santiago de Cuba",
                ActiveDoctors = 22,
                AvgReportsPerDoctor = 13.1,
                AvgReviewTimeHours = 29.8,
                AvgAssignmentHours = 6.7,

                Municipalities = new List<MunicipalityMedicalActivityDto>
                {
                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Santiago de Cuba",
                        ActiveDoctors = 10,
                        AvgReportsPerDoctor = 15.2,
                        AvgReviewTimeHours = 27.3,
                        AvgAssignmentHours = 5.4
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Contramaestre",
                        ActiveDoctors = 5,
                        AvgReportsPerDoctor = 11.8,
                        AvgReviewTimeHours = 31.6,
                        AvgAssignmentHours = 7.8
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Palma Soriano",
                        ActiveDoctors = 4,
                        AvgReportsPerDoctor = 9.7,
                        AvgReviewTimeHours = 33.5,
                        AvgAssignmentHours = 8.1
                    }
                }
            },

            new ProvinceMedicalActivityDto
            {
                ProvinceName = "Villa Clara",
                ActiveDoctors = 19,
                AvgReportsPerDoctor = 12.9,
                AvgReviewTimeHours = 25.1,
                AvgAssignmentHours = 5.0,

                Municipalities = new List<MunicipalityMedicalActivityDto>
                {
                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Santa Clara",
                        ActiveDoctors = 9,
                        AvgReportsPerDoctor = 14.4,
                        AvgReviewTimeHours = 22.8,
                        AvgAssignmentHours = 4.3
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Placetas",
                        ActiveDoctors = 4,
                        AvgReportsPerDoctor = 10.2,
                        AvgReviewTimeHours = 28.1,
                        AvgAssignmentHours = 5.9
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Sagua la Grande",
                        ActiveDoctors = 3,
                        AvgReportsPerDoctor = 8.7,
                        AvgReviewTimeHours = 30.4,
                        AvgAssignmentHours = 6.2
                    }
                }
            },

            new ProvinceMedicalActivityDto
            {
                ProvinceName = "Camagüey",
                ActiveDoctors = 17,
                AvgReportsPerDoctor = 11.3,
                AvgReviewTimeHours = 31.4,
                AvgAssignmentHours = 7.3,

                Municipalities = new List<MunicipalityMedicalActivityDto>
                {
                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Camagüey",
                        ActiveDoctors = 8,
                        AvgReportsPerDoctor = 12.5,
                        AvgReviewTimeHours = 28.6,
                        AvgAssignmentHours = 6.1
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Florida",
                        ActiveDoctors = 3,
                        AvgReportsPerDoctor = 9.1,
                        AvgReviewTimeHours = 34.2,
                        AvgAssignmentHours = 8.0
                    }
                }
            },

            new ProvinceMedicalActivityDto
            {
                ProvinceName = "Holguín",
                ActiveDoctors = 16,
                AvgReportsPerDoctor = 10.6,
                AvgReviewTimeHours = 27.9,
                AvgAssignmentHours = 6.0,

                Municipalities = new List<MunicipalityMedicalActivityDto>
                {
                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Holguín",
                        ActiveDoctors = 7,
                        AvgReportsPerDoctor = 12.2,
                        AvgReviewTimeHours = 25.4,
                        AvgAssignmentHours = 5.1
                    },

                    new MunicipalityMedicalActivityDto
                    {
                        MunicipalityName = "Banes",
                        ActiveDoctors = 3,
                        AvgReportsPerDoctor = 8.9,
                        AvgReviewTimeHours = 31.2,
                        AvgAssignmentHours = 7.2
                    }
                }
            }
        }
        };
    }


    public async Task<AdminVaccineDashboardDto> GetVaccinesAsync()
    {
        return new AdminVaccineDashboardDto
        {
            Vaccines = new List<VaccineStatusDto>
        {
            new VaccineStatusDto
            {
                VaccineName = "Abdala",
                TotalReports = 248,

                Lots = new List<LotsStatusDto>
                {
                    new LotsStatusDto
                    {
                        LotNumber = "ABD-2025-001",
                        TotalReports = 74
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "ABD-2025-002",
                        TotalReports = 63
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "ABD-2025-003",
                        TotalReports = 58
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "ABD-2025-004",
                        TotalReports = 53
                    }
                }
            },

            new VaccineStatusDto
            {
                VaccineName = "Soberana 02",
                TotalReports = 191,

                Lots = new List<LotsStatusDto>
                {
                    new LotsStatusDto
                    {
                        LotNumber = "SOB02-310A",
                        TotalReports = 69
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "SOB02-311B",
                        TotalReports = 51
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "SOB02-312C",
                        TotalReports = 42
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "SOB02-313D",
                        TotalReports = 29
                    }
                }
            },

            new VaccineStatusDto
            {
                VaccineName = "Soberana Plus",
                TotalReports = 142,

                Lots = new List<LotsStatusDto>
                {
                    new LotsStatusDto
                    {
                        LotNumber = "PLUS-110A",
                        TotalReports = 48
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "PLUS-111B",
                        TotalReports = 37
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "PLUS-112C",
                        TotalReports = 31
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "PLUS-113D",
                        TotalReports = 26
                    }
                }
            },

            new VaccineStatusDto
            {
                VaccineName = "Mambisa",
                TotalReports = 97,

                Lots = new List<LotsStatusDto>
                {
                    new LotsStatusDto
                    {
                        LotNumber = "MAM-220A",
                        TotalReports = 34
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "MAM-221B",
                        TotalReports = 28
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "MAM-222C",
                        TotalReports = 20
                    },

                    new LotsStatusDto
                    {
                        LotNumber = "MAM-223D",
                        TotalReports = 15
                    }
                }
            }
        },

            SymptomDistribution = new List<SymptomDistributionDto>
        {
            new SymptomDistributionDto
            {
                SymptomName = "Fiebre",
                Count = 212,
                Percentage = 24.8
            },

            new SymptomDistributionDto
            {
                SymptomName = "Dolor en el sitio de inyección",
                Count = 187,
                Percentage = 21.9
            },

            new SymptomDistributionDto
            {
                SymptomName = "Cefalea",
                Count = 143,
                Percentage = 16.7
            },

            new SymptomDistributionDto
            {
                SymptomName = "Fatiga",
                Count = 118,
                Percentage = 13.8
            },

            new SymptomDistributionDto
            {
                SymptomName = "Mialgia",
                Count = 82,
                Percentage = 9.6
            },

            new SymptomDistributionDto
            {
                SymptomName = "Escalofríos",
                Count = 61,
                Percentage = 7.1
            },

            new SymptomDistributionDto
            {
                SymptomName = "Náuseas",
                Count = 33,
                Percentage = 3.9
            },

            new SymptomDistributionDto
            {
                SymptomName = "Mareo",
                Count = 19,
                Percentage = 2.2
            }
        }
        };
    }

}