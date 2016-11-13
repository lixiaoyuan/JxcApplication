using System.Collections.ObjectModel;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.Models
{
    public class CategoriesOutOrder
    {
        public ProductOutStorage OutStorage { get; set; }
        public ObservableCollection<ProductOutStorageDetail> OutStorageDetails { get; set; }
    }
}
