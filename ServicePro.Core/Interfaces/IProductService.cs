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
        Task<List<getallProductResponseDTO>> getallProductglobaleResponseDTO();
        Task<List<ProductResponseDTO>> GetAllinactiveProductsAsync();

        Task<List<CategoryWithProductsDTO>> GetProductsByCategoryAsync();
        Task<Alltabledataforlisting> getallinactiveproducts(Guid id);
        Task<Alltabledataforlisting> GetActiveProduct(Guid id);



        Task<List<Alltabledataforlisting>> GetAllProductsAsyncbyproductsandimageid();
        Task<ProductResponseDTO> UpdateProductAsync(Guid id, uupdateProductonlyproductstabledataDTO dto);
        Task<string> UpdateSingleImageAsync(UpdateSingleImageDTO dto);
        Task<bool> UpdateProductStatusAsync(Guid id, bool isActive);
    }

}
