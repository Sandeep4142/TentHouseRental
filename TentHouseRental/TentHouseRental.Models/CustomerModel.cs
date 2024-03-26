using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentHouseRental.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public virtual ICollection<TransactionHistoryModel> TransactionHistories { get; set; } = new List<TransactionHistoryModel>();
    }
}
