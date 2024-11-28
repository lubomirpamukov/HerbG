using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Review;

public class ReviewViewModel
{
    public string ReviewerName { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public int Rating { get; set; }
}

