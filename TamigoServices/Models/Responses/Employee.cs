using System;

namespace TamigoServices.Models.Responses
{
    public class Employee
    {
        public DepartmentRole[] DepartmentRoles { get; set; }
        public string Email { get; set; }
        public Guid EmployeeId { get; set; }
        public object EmployerNumber { get; set; }
        public object EndDate { get; set; }
        public string HomeDepartment { get; set; }
        public Guid HomeDepartmentId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPlanner { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public object[] PosKeys { get; set; }
        public object Role { get; set; }
        public string WageRateTypeName { get; set; }
        public string WageSystemKey { get; set; }
    }
}