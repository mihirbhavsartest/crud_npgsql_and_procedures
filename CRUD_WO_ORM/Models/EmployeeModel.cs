using System.ComponentModel.DataAnnotations;

namespace CRUD_WO_ORM.Models
{
    public class Employee
    {
        [Key] 
        public int EmployeeId { get; set; }
        
        [Required]
        public string EmployeeName { get; set; }
        
        [Required]
        public string EmployeeDOB { get; set; } = Convert.ToString(DateOnly.FromDateTime(DateTime.Now));
        
        [Required]
        public string EmployeeFunction { get; set; }

        [Required]
        public string EmployeeLocation { get; set; }

        [Required]
        public string EmployeeRole { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
