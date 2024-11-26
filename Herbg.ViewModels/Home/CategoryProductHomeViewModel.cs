using Herbg.ViewModels.Category;
using Herbg.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Home;

public class CategoryProductHomeViewModel
{
    public ICollection<CategoryCardViewModel> Categories { get; set; } = new List<CategoryCardViewModel>();

    public ICollection<ProductCardViewModel> Products { get; set; } = new List<ProductCardViewModel>();
}
