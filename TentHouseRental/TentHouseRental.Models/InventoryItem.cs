using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentHouseRental.Models
{
    public class InventoryItem
    {
        public string? ItemName { get; set; }
        public int AvailableQuantity { get; set; }
        public List<TransactionHistoryModel>? Transactions { get; set; }
    }
}
