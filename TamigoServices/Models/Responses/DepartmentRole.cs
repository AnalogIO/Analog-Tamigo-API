using System;

namespace TamigoServices.Models.Responses
{
    public class DepartmentRole
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}