using System.Collections.ObjectModel;

namespace BusinessDb.Cor.Models
{
    public class CategoriesOrder<TStorage,TStorageDetail>
    {
        public TStorage MasterStorage { get; set; }
        public ObservableCollection<TStorageDetail> Details { get; set; }
    }
}
