using System;

namespace TamigoServices.Models.Responses
{
    public class UserLoginResponse
    {
        public Guid DefaultCompanyId { get; set; }
        public string DefaultCompanyName { get; set; }
        public Guid DefaultDepartmentId { get; set; }
        public string DefaultDepartmentName { get; set; }
        public string Email { get; set; }
        public Guid EmployeeId { get; set; }
        public string ImageUrl { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public Guid SessionToken { get; set; }
    }
}