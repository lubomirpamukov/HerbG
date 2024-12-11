using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Manufactorer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Services;

public class ManufactorerService(IRepository<Manufactorer>manufactorers) : IManufactorerService
{
    private readonly IRepository<Manufactorer> _manufactorers = manufactorers;

    public ICollection<Manufactorer> GetAllManufactorersDbModel()
    {
        return _manufactorers
            .GetAllAttached()
            .ToArray();
    }

    public async Task<IEnumerable<string>> GetManufactorersNamesAsync()
    {
        return await _manufactorers
            .GetAllAttached()
            .Select(x => x.Name)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<ManufacturerViewModel>> GetManufacturersAsync()
    {
        return await _manufactorers
            .GetAllAttached()
            .Select(m => new ManufacturerViewModel
            {
                Id = m.Id,
                Name = m.Name
            })
            .ToListAsync();
    }

}
