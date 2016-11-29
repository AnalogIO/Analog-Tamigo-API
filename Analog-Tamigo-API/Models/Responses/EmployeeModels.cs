namespace Analog_Tamigo_API.Models.Responses
{
    public class Employee
    {
        public Departmentrole[] DepartmentRoles { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public object EmployerNumber { get; set; }
        public object EndDate { get; set; }
        public string HomeDepartment { get; set; }
        public string HomeDepartmentId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPlanner { get; set; }
        public string Name { get; set; }
        public object Phone { get; set; }
        public object[] PosKeys { get; set; }
        public object Role { get; set; }
        public string WageRateTypeName { get; set; }
        public string WageSystemKey { get; set; }
    }

    public class Departmentrole
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class DetailedEmployee
    {
        public string About { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string BankAccount { get; set; }
        public string BankRegistration { get; set; }
        public string Birthdate { get; set; }
        public bool CanAddOwnShiftsOnSmartphone { get; set; }
        public bool CanApproveEmployeeChanges { get; set; }
        public bool CanEditActualHours { get; set; }
        public bool CanEditPlan { get; set; }
        public bool CanSendSms { get; set; }
        public string CarRegistration { get; set; }
        public string City { get; set; }
        public object[] Competencies { get; set; }
        public string CurrentWageRateType { get; set; }
        public object[] CustomColumns { get; set; }
        public object[] CustomRoles { get; set; }
        public object DeletedOn { get; set; }
        public string EmployeeId { get; set; }
        public bool EmployeeIsAdmin { get; set; }
        public string EmployeeType { get; set; }
        public string EmployerNumber { get; set; }
        public bool EnforceMaxWeeklyWorkingHours { get; set; }
        public object From { get; set; }
        public object HeadCount { get; set; }
        public Historicalwage[] HistoricalWages { get; set; }
        public string HomeDepartment { get; set; }
        public string IdNumber { get; set; }
        public bool IsUserEnabled { get; set; }
        public object MaximumHours { get; set; }
        public object MaximumWageRate { get; set; }
        public object MinimumHours { get; set; }
        public string Name { get; set; }
        public object[] NotAvailable { get; set; }
        public object[] Notes { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string ReasonForTermination { get; set; }
        public Role[] Roles { get; set; }
        public bool SendShiftReminder { get; set; }
        public object StandardHours { get; set; }
        public object To { get; set; }
        public string WageNumber { get; set; }
        public object WorkTravelTime { get; set; }
        public string Zip { get; set; }
    }

    public class Historicalwage
    {
        public string StartDate { get; set; }
        public int Wage { get; set; }
    }

    public class Role
    {
        public bool Admin { get; set; }
        public string DepartmentId { get; set; }
        public bool Employee { get; set; }
        public bool Planner { get; set; }
    }

}