using Herbg.Models;
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

    public ICollection<Category> GetAllCategoriesDbModelAsync();

    public  Task<IEnumerable<string>> GetCategoriesNamesAsync();

    public Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

    public Task<bool> AddCategoryAsync(CategoryCardViewModel model);

    public Task<CategoryCardViewModel?> GetCategoryByIdAsync(int categoryId);
    public Task<bool> EditCategoryAsync(CategoryCardViewModel model);

    public Task<bool> DeleteCategoryAsync(int categoryId);
}
