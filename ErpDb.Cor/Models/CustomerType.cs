using System.ComponentModel.DataAnnotations;

namespace BusinessDb.Cor.Models
{
    public enum CustomerType 
    {
        [Display(Description = "零售商")]
        Retail,
        [Display(Description = "经销商")]
        Dealer,
        [Display(Description = "网销商")]
        Intnet
    }
}