using ServicePro.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO> CreateProductAsync(CreateProductDTO dto);
        Task<List<ProductResponseDTO>> GetAllProductsAsync();
        Task<List<CategoryWithProductsDTO>> GetProductsByCategoryAsync();

        Task<List<Alltabledataforlisting>> GetAllProductsAsyncbyproductsandimageid();
        Task<ProductResponseDTO> UpdateProductAsync(Guid id, uupdateProductDTO dto);
        Task<string> UpdateSingleImageAsync(UpdateSingleImageDTO dto);
    }

}
