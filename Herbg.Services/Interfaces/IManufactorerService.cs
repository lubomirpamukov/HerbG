using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface IManufactorerService
{
    public Task<IEnumerable<string>> GetManufactorersNamesAsync();
}
