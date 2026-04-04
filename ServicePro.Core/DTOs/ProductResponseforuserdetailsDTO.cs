using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.DTOs
{
    public class ProductResponseforuserdetailsDTO
    {
      
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Description { get; set; }
    
        // ✅ THIS WAS MISSING
        public List<ProductVariantDto> ProductVariants { get; set; }
    }
}
