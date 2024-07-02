using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyMVC.ViewModels
{
    public class DepartmentViewModel
    {

        public int Id { get; set; }

        [MaxLength(10)]
        public string? Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Display(Name = "Date of Creation")]
        public DateTime DateOfCreation { get; set; }

        // Navigation property
        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    }
}
