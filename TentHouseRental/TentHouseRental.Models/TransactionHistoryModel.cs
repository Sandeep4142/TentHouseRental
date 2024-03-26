using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentHouseRental.Models
{
    public class TransactionHistoryModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public bool TransactionType { get; set; }
        public int Quantity { get; set; }
        public int? TransactionParentId { get; set; }

    }
}
