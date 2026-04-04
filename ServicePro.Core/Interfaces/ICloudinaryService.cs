using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.Interfaces
{
    public interface ICloudinaryService
    {
        Task<(string url, string publicId)> UploadImageAsync(IFormFile file);
        Task DeleteImageAsync(string publicId);
    }

}
