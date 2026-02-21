using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.DTOs
{
    public class UpdateSingleImageDTO
    {
        public Guid ProductId { get; set; }
        public Guid ProductImageId { get; set; }
        public IFormFile NewImage { get; set; }
    }
    public class uupdateProductDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
