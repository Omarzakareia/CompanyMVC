using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CompanyMVC.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; } //pk
        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 character")]
        [MinLength(5, ErrorMessage = "Min Length is 5 Characters")]
        public string? Name { get; set; }
        [Range(22, 35, ErrorMessage = "Age Must be Between 22 to 35")]
        public int? Age { get; set; }
        //123-street-city-country
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{4,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address Must Be Like 123-street-city-country ")]
        public string? Address { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "${0:N2}")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; } 
        public string? ImageName { get; set; }
        public DateTime HireDate { get; set; }
        [ForeignKey("Department")]

        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department? Department { get; set; }

    }
}
