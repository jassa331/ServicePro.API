using Microsoft.EntityFrameworkCore;
using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using ServicePro.Core.Interfaces;
using ServicePro.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinary;

        public ProductService(AppDbContext context, ICloudinaryService cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        public async Task<ProductResponseDTO> CreateProductAsync(CreateProductDTO dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Category = dto.Category,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            foreach (var image in dto.Images)
            {
                var uploadResult = await _cloudinary.UploadImageAsync(image);

                var productImage = new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ImageUrl = uploadResult.url,
                    PublicId = uploadResult.publicId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ProductImages.Add(productImage);
            }

            await _context.SaveChangesAsync();

            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageUrls = _context.ProductImages
                    .Where(x => x.ProductId == product.Id)
                    .Select(x => x.ImageUrl)
                    .ToList()
            };
        }

        public async Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive)
                .Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    ImageUrls = p.ProductImages.Select(x => x.ImageUrl).ToList()
                }).ToListAsync();
        }
        public async Task<List<CategoryWithProductsDTO>> GetProductsByCategoryAsync()
        {
            var products = await _context.Products
                         .Include(p => p.ProductImages)
                         .Where(p => p.IsActive)
                         .ToListAsync();


            return products
                .GroupBy(p => p.Category)
                .Select(group => new CategoryWithProductsDTO
                {
                    Category = group.Key,
                    Products = group.Select(p => new ProductResponseDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Category = p.Category,
                        ImageUrls = p.ProductImages.Select(i => i.ImageUrl).ToList()
                    }).ToList()
                })
                .ToList();
        }


    }

}
