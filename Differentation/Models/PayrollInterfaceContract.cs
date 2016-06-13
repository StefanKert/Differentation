using System;

namespace Rota.PayrollInterface.Models
{
    public enum VacationUnit
    {
        WeekDay, // Werktag
        WorkingDay, // Arbeitstag
        Hour
    }

    public enum SalaryType
    {
        MonthBased,
        HourBased
    }


    public class PiContract
    {
        public string Employment { get; set; }

        public string PlaceOfEmployment { get; set; }

        public double WeeklyWorkingDays { get; set; }

        public string AdditionalInformation { get; set; }

        public double FixedOvertimeRate { get; set; }

        public double WagePerHour { get; set; }

        public double HourBase { get; set; }

        public string BaseContract { get; set; }

        public double TotalGrossWage { get; set; }

        public double TotalNetWage { get; set; }

        public double OvertimeMultiplicator { get; set; }

        public double OvertimeDelemiter { get; set; }

        public string JobDescription { get; set; }

        public bool Worker { get; set; }

        public string WithDrawalType { get; set; }

        public DateTime BeginOfContract { get; set; }

        public DateTime EndOfContract { get; set; }

        public double VacationDemandInWeeksPerYear { get; set; }

        public string Insurance { get; set; }

        public VacationUnit VacationUnit { get; set; }

        public SalaryType SalaryType { get; set; }
    }
}