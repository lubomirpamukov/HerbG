using Herbg.Models;
using Herbg.ViewModels.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface IReviewService
{
    public Task<ReviewViewModel> GetReviewFormAsync(string clientId,int productId);

    public Task<bool> UpdateReviewAsync(string clientId, ReviewViewModel model);
}
