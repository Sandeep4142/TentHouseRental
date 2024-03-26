using System;
using System.Collections.Generic;

namespace TentHouseRental.DAL.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductTitle { get; set; } = null!;

    public int QuantityTotal { get; set; }

    public int QuantityBooked { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
