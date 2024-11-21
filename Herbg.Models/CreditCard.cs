using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.CreditCard;

namespace Herbg.Models;

public class CreditCard
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [StringLength(CardNumberMaxLength, MinimumLength = CardNumberMinLength, ErrorMessage = CardNumberErrorMessage)]
    [DataType(DataType.CreditCard)]
    public string CardNumber { get; set; } = null!;

    [Required]
    [StringLength(CardHolderNameMaxLength, MinimumLength = CardHolderNameMinLength, ErrorMessage = CardHolderNameErrorMessage)]
    public string CardHolderName { get; set; } = null!;

    [Required]
    [RegularExpression(@"\d{3,4}", ErrorMessage = CVVErrorMessage)]
    public string CVV { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ExpirationDateFormat)]
    public DateTime ExpirationDate { get; set; }

    public bool IsDeleted { get; set; }

    [Required]
    public string ClientId { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(ClientId))]
    public virtual ApplicationUser Client { get; set; } = null!;

    [Required]
    required public string OrderId { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;
}
