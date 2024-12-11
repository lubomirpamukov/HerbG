using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Category;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Services;

public class CategoryService(IRepository<Category> category, IRepository<Product>product) : ICategoryService
{
    private readonly IRepository<Category> _category = category;
    private readonly IRepository<Product> _product = product;
    public async Task<ICollection<CategoryCardViewModel>> GetAllCategoriesAsync()
    {
        var categories = await _category
            .GetAllAttached()
            .Where(c => c.IsDeleted == false)
            .Select(c => new CategoryCardViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ImagePath = c.ImagePath!,
                Description = c.Description
            })
            .ToArrayAsync();
        return categories;
    }

    public ICollection<Category> GetAllCategoriesDbModelAsync() 
    {
        var categories = _category
            .GetAllAttached()
            .Where(c => c.IsDeleted == false)
            .ToArray();
        return categories;

    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
    {
        return await _category
            .GetAllAttached()
            .Where(c => c.IsDeleted == false)
            .Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }


    public async Task<IEnumerable<string>> GetCategoriesNamesAsync()
    {
        return await _category.GetAllAttached()
            .Where(c => c.IsDeleted)
            .Select(c => c.Name)
            .ToArrayAsync();
    }

    public async Task<bool> AddCategoryAsync(CategoryCardViewModel model)
    {
        try
        {
            var categoryToAdd = new Category
            {
                Name = model.Name,
                ImagePath = model.ImagePath,
                Description = model.Description,
            };

            await _category.AddAsync(categoryToAdd); // Save the new category to the database
            return true; // Return true on success
        }
        catch (Exception)
        {
            return false; // Return false if an error occurs
        }
    }

    public async Task<CategoryCardViewModel?> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _category
            .GetAllAttached()
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryCardViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ImagePath = c.ImagePath!,
                Description = c.Description
            })
            .FirstOrDefaultAsync();

        return category;
    }

    public async Task<bool> EditCategoryAsync(CategoryCardViewModel model)
    {
        var categoryToEdit = await _category
            .GetAllAttached()  // or your specific method to get data
            .FirstOrDefaultAsync(c => c.Id == model.Id);

        if (categoryToEdit == null)
        {
            return false; 
        }

        categoryToEdit.Name = model.Name;
        categoryToEdit.Description = model.Description;
        categoryToEdit.ImagePath = model.ImagePath;

        await _category.UpdateAsync(categoryToEdit);
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var categoryToDelete = await _category
            .GetAllAttached()  // or your method to retrieve categories
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (categoryToDelete == null)
        {
            return false; // Category not found
        }

        // Set the category as deleted (soft delete)
        categoryToDelete.IsDeleted = true;

        // Find all products in this category and mark them as deleted
        var productsInCategory = await _product
            .GetAllAttached()
            .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
            .ToListAsync();

        foreach (var product in productsInCategory)
        {
            product.IsDeleted = true; // Soft delete product
        }

        // Update the category and products in the database
        await _category.UpdateAsync(categoryToDelete);

        _product.UpdateRange(productsInCategory); 
        await _product.SaveChangesAsync();

        return true; // Deletion successful
    }
}
