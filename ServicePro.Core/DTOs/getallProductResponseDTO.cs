using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.DTOs
{
    public class getallProductResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public List<ProductImageDto> ProductImages { get; set; }
        public List<ProductVariantDto>ProductVariants { get; set; }


    }
}

