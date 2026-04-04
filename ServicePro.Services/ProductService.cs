using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using ServicePro.Core.Interfaces;
using ServicePro.Infrastructure.Data;
using System.Collections.Generic;
using System.Data;
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
            var variants = new List<ProductVariantDto>();

            if (dto.VariantsJson != null && dto.VariantsJson.Length > 0)
            {
                foreach (var item in dto.VariantsJson)
                {
                    if (item.Trim().StartsWith("["))
                    {
                        // JSON ARRAY
                        var list = JsonConvert.DeserializeObject<List<ProductVariantDto>>(item);
                        variants.AddRange(list);
                    }
                    else
                    {
                        // JSON OBJECT
                        var single = JsonConvert.DeserializeObject<ProductVariantDto>(item);
                        variants.Add(single);
                    }
                }
            }

            foreach (var variant in variants)
            {
                var productVariant = new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Weight = variant.Weight,
                    OriginalPrice = variant.OriginalPrice,
                    SellPrice = variant.SellPrice,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ProductVariants.Add(productVariant);
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
        public async Task<bool> UpdateProductStatusAsync(Guid id, bool isActive)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            // Update IsActive value
            product.IsActive = isActive;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<ProductResponseDTO>> GetAllinactiveProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == false)
                .Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    ImageUrls = p.ProductImages.Select(x => x.ImageUrl).ToList(),
                    Description = p.Description
                }).ToListAsync();
        }
        public async Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == true)
                .Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category,
                    ImageUrls = p.ProductImages.Select(x => x.ImageUrl).ToList(),
                    Description = p.Description
                }).ToListAsync();
        }
        public async Task<List<getallProductResponseDTO>> getallProductglobaleResponseDTO()
        {
            var result = new List<getallProductResponseDTO>();

            var connection = _context.Database.GetDbConnection();

            await using (var command = connection.CreateCommand())
            {
                command.CommandText = "GetAllProductsWithDetails";
                command.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var table = new List<dynamic>();

                    while (await reader.ReadAsync())
                    {
                        table.Add(new
                        {
                            Id = reader["Id"],
                            Name = reader["Name"]?.ToString(),
                            Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                            Category = reader["Category"]?.ToString(),
                            Description = reader["Description"]?.ToString(),

                            ImageId = reader["ImageId"] == DBNull.Value ? null : reader["ImageId"],
                            ProductId = reader["ProductId"] == DBNull.Value ? null : reader["ProductId"],
                            ImageUrl = reader["ImageUrl"]?.ToString(),
                            PublicId = reader["PublicId"]?.ToString(),
                            CreatedAt = reader["CreatedAt"] == DBNull.Value ? null : reader["CreatedAt"],

                            VariantId = reader["VariantId"] == DBNull.Value ? null : reader["VariantId"],
                            Weight = reader["Weight"]?.ToString(),
                            OriginalPrice = reader["OriginalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["OriginalPrice"]) : 0,
                            SellPrice = reader["SellPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellPrice"]) : 0
                        });
                    }

                    // ✅ GROUP DATA PROPERLY
                    result = table
                        .GroupBy(x => x.Id)
                        .Select(g => new getallProductResponseDTO
                        {
                            Id = g.Key,
                            Name = g.First().Name,
                            Price = g.First().Price,
                            Category = g.First().Category,
                            Description = g.First().Description,

                            // ✅ Images
                            ProductImages = g
                                .Where(x => x.ImageId != null)
                                .Select(x => new ProductImageDto
                                {
                                    Id = x.ImageId,
                                    ProductId = x.Id,
                                    ImageUrl = x.ImageUrl,
                                    PublicId = x.PublicId,
                                    CreatedAt = x.CreatedAt ?? DateTime.Now
                                })
                                .GroupBy(i => i.Id)
                                .Select(i => i.First())
                                .ToList(),

                            // ✅ Variants
                                    ProductVariants = g
                                   .Where(x => x.VariantId != null)
                                   .GroupBy(x => x.VariantId)
                                   .Select(v => new ProductVariantDto
                                   {
                                       Weight = v.First().Weight,
                                       OriginalPrice = v.First().OriginalPrice,
                                       SellPrice = v.First().SellPrice
                                   })
                                   .ToList()

                        })
                        .ToList();
                }
            }

            return result;
        }
        //public async Task<List<getallProductResponseDTO>> getallProductglobaleResponseDTO()
        //{
        //    return await _context.Products
        //        .Include(p => p.ProductImages)
        //        .Include(p => p.ProductVariants)   // ✅ Your navigation property
        //        .Where(p => p.IsActive == true)
        //        .Select(p => new getallProductResponseDTO
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //            Price = p.Price,
        //            Category = p.Category,

        //            // ✅ Map Images
        //            ProductImages = p.ProductImages.Select(img => new ProductImageDto
        //            {
        //                Id = img.Id,
        //                ProductId = img.ProductId,
        //                ImageUrl = img.ImageUrl,
        //                PublicId = img.PublicId,
        //                CreatedAt = img.CreatedAt
        //            }).ToList(),

        //            // ✅ Map Variants (FIXED → DTO)
        //            ProductVariants = p.ProductVariants.Select(v => new ProductVariantDto
        //            {
        //                Weight = v.Weight,
        //                OriginalPrice = v.OriginalPrice,
        //                SellPrice = v.SellPrice
        //            }).ToList(),

        //            Description = p.Description
        //        })
        //        .ToListAsync();
        //}
        public async Task<Alltabledataforlisting?> getallinactiveproducts(Guid id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id && p.IsActive == false)
                .Select(p => new Alltabledataforlisting
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Category = p.Category,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,

                    ProductImages = p.ProductImages.Select(img => new ProductImageDto
                    {
                        Id = img.Id,
                        ProductId = img.ProductId,
                        ImageUrl = img.ImageUrl,
                        PublicId = img.PublicId,
                        CreatedAt = img.CreatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<Alltabledataforlisting?> GetActiveProduct(Guid id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id && p.IsActive == true)
                .Select(p => new Alltabledataforlisting
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Category = p.Category,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,

                    ProductImages = p.ProductImages.Select(img => new ProductImageDto
                    {
                        Id = img.Id,
                        ProductId = img.ProductId,
                        ImageUrl = img.ImageUrl,
                        PublicId = img.PublicId,
                        CreatedAt = img.CreatedAt
                    }).ToList(),
                    productVariant = p.ProductVariants
                            .Select(v => new GetactiveProductVariantdto
                            {
                                ProductId=v.ProductId,
                                Id=v.Id,
                                IsActive=v.IsActive,
                                Weight = v.Weight,
                                OriginalPrice = v.OriginalPrice,
                                SellPrice = v.SellPrice
                            })
                            .ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }
        public async Task<List<Alltabledataforlisting>> GetAllProductsAsyncbyproductsandimageid()
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .ToListAsync();

            var result = products.Select(p => new Alltabledataforlisting
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Category = p.Category,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,

                ProductImages = p.ProductImages.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ProductId = img.ProductId,
                    ImageUrl = img.ImageUrl,
                    PublicId = img.PublicId,
                    CreatedAt = img.CreatedAt
                }).ToList()
            }).ToList();

            return result;
        }


        public async Task<List<CategoryWithProductsDTO>> GetProductsByCategoryAsync()
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)   
                .Where(p => p.IsActive == true)
                .ToListAsync();

            return products
                .GroupBy(p => p.Category)
                .Select(group => new CategoryWithProductsDTO
                {
                    Category = group.Key,
                    Products = group.Select(p => new ProductResponseforuserdetailsDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Category = p.Category,
                        Description = p.Description,

                        // ✅ Images same rahenge
                        ImageUrls = p.ProductImages
                                     .Select(i => i.ImageUrl)
                                     .ToList(),

                        ProductVariants = p.ProductVariants
                            .Select(v => new ProductVariantDto
                            {
                                Weight = v.Weight,
                                OriginalPrice = v.OriginalPrice,
                                SellPrice = v.SellPrice
                            })
                            .ToList()

                    }).ToList()
                })
                .ToList();
        }

        public async Task<ProductResponseDTO> UpdateProductAsync(Guid id, uupdateProductonlyproductstabledataDTO dto)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new Exception("Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Category = dto.Category;
            product.IsActive = dto.isactive;
            product.UpdatedAt = DateTime.UtcNow;
            var variants = new List<UpdateProductVariantDTO>();

            if (dto.Variants != null && dto.Variants.Length > 0)
            {
                foreach (var json in dto.Variants)
                {
                    if (string.IsNullOrWhiteSpace(json))
                        continue;

                    var variant = JsonConvert.DeserializeObject<UpdateProductVariantDTO>(json);
                    variants.Add(variant);
                }
            }
            // Parse VariantsJson
            //var variants = new List<UpdateProductVariantDTO>();

            //if (dto.Variants != null && dto.Variants.Length > 0)
            //{
            //    foreach (var item in dto.Variants)
            //    {
            //        if (string.IsNullOrWhiteSpace(item))
            //            continue;

            //        if (item.Trim().StartsWith("["))
            //        {
            //            var list = JsonConvert.DeserializeObject<List<UpdateProductVariantDTO>>(item);
            //            variants.AddRange(list);
            //        }
            //        else
            //        {
            //            var single = JsonConvert.DeserializeObject<UpdateProductVariantDTO>(item);
            //            variants.Add(single);
            //        }
            //    }
            //}

            // Update Variants
            foreach (var variantDto in variants)
            {
                var variant = product.ProductVariants
                    .FirstOrDefault(v => v.Id == variantDto.Id);

                if (variant != null)
                {
                    variant.Weight = variantDto.Weight;
                    variant.OriginalPrice = variantDto.OriginalPrice;
                    variant.SellPrice = variantDto.SellPrice;
                }
            }

            await _context.SaveChangesAsync();

            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name
            };
        }


        public async Task<string> UpdateSingleImageAsync(UpdateSingleImageDTO dto)
        {
            var image = await _context.ProductImages
                .FirstOrDefaultAsync(x => x.Id == dto.ProductImageId
                                       && x.ProductId == dto.ProductId);

            if (image == null)
                throw new Exception("Image not found");

            await _cloudinary.DeleteImageAsync(image.PublicId);

            var uploadResult = await _cloudinary.UploadImageAsync(dto.NewImage);

            image.ImageUrl = uploadResult.url;
            image.PublicId = uploadResult.publicId;
            image.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return "Image updated successfully";
        }
    }
}