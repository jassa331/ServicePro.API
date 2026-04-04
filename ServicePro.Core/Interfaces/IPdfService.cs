using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePro.Core.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateContactPdf(List<ContactResponseDto> contacts);

    }
}
