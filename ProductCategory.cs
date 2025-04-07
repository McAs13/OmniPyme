using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmniPyme.Web.Data.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
