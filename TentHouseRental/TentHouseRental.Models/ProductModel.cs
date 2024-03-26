using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentHouseRental.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = null!;
        public int QuantityTotal { get; set; }
        public int QuantityBooked { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<TransactionHistoryModel> TransactionHistories { get; set; } = new List<TransactionHistoryModel>();
    }
}
