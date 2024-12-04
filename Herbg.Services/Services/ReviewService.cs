
using Herbg.Infrastructure.Interfaces;
using Herbg.Models;
using Herbg.Services.Interfaces;
using Herbg.ViewModels.Review;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Services;

public class ReviewService(IRepositroy<ApplicationUser>client,IRepositroy<Review>review) : IReviewService
{
    private readonly IRepositroy<ApplicationUser> _client = client;
    private readonly IRepositroy<Review> _review = review;
    public async Task<ReviewViewModel> GetReviewFormAsync(string clientId, int productId)
    {
        var client = await _client.FindByIdAsync(clientId);
        //Create a view form
        var viewForm = new ReviewViewModel { Id = productId, ReviewerName = client?.Email ?? "Anonymouse", };
        return viewForm;
    }

    public async Task<bool> UpdateReviewAsync(string clientId, ReviewViewModel model)
    {
        var client = await _client.FindByIdAsync(clientId);

        //Check if customer wrote a review for this product
        var customerReview = await _review
            .GetAllAttachedAsync()
            .FirstOrDefaultAsync(review => review.ProductId == model.Id && review.ClientId == clientId);
        if (customerReview != null)
        {
            //Delete old review
            await _review.DeleteAsync(customerReview);
        }

        //Create a review from the viewModel
        var newReview = new Review
        {
            ClientId = clientId,
            Description = model.Description,
            ProductId = model.Id,
            Rating = model.Rating,
        };

        await _review.AddAsync(newReview);
        return true;
    }
}
