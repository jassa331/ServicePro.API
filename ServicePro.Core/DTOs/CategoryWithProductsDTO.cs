using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.DTOs
{
    public class CategoryWithProductsDTO
    {
        public string Category { get; set; }
        public List<ProductResponseDTO> Products { get; set; }
    }

}
