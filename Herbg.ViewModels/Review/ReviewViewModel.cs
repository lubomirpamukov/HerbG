using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Review;

public class ReviewViewModel
{
    public int Id { get; set; }
    public string ReviewerName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Rating { get; set; }
}

