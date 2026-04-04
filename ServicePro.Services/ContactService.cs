using Microsoft.EntityFrameworkCore;
using ServicePro.Core.DTOs;
using ServicePro.Core.Entities;
using ServicePro.Core.Interfaces;
using ServicePro.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ServicePro.Services;

public class ContactService : IContactService
{
    private readonly AppDbContext _context;

    public ContactService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateContactAsync(ContactDto dto)
    {
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Product = dto.Product,
            Message = dto.Message
        };

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
    }
    public async Task<List<ContactResponseDto>> GetAllContactsAsync()
    {
        return await _context.Contacts
            .Select(x => new ContactResponseDto
            {
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Product = x.Product,
                Message = x.Message,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }
}

