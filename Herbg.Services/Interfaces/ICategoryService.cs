using Herbg.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface ICategoryService
{
    public Task<ICollection<CategoryCardViewModel>> GetAllCategoriesAsync();

    public  Task<IEnumerable<string>> GetCategoriesNamesAsync();
}
