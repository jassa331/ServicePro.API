using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.DTOs
{
    public class GetactiveProductVariantdto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Weight { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal SellPrice { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
