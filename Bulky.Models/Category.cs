using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]                   // 13- Maximum Length Validation
        [DisplayName("Category Name")]    // Data Annotation
        public string? Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Message must be between 1-100")]      //15- Custom Error Message   // Display order validation // For Implementing validation and showing error ----- head to Create.cshtml
        public int DisplayOrder { get; set; }
    }
}
