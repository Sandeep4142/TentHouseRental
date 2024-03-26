using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentHouseRental.DAL.Models;
using TentHouseRental.Models;
using TentHouseRental.Utils;

namespace TentHouseRental.DAL
{
    public class TransactionHistoryDA : ITransactionHistoryDA
    {
        private readonly TentHouseRentalContext context;
        public TransactionHistoryDA(TentHouseRentalContext context, ILogger logger)
        {
            this.context = context;
        }

        public async Task<List<TransactionHistory>> GetAllTransaction()
        {
            List<TransactionHistory> transactionList = await context.TransactionHistories
                                                .OrderByDescending(t => t.TransactionDateTime)
                                                .ToListAsync();
            return transactionList;
        }

        public async Task<(bool, int)> AddTransaction(TransactionHistory transaction)
        {
            var lastTransaction = await context.TransactionHistories
                .OrderByDescending(t => t.TransactionDateTime)
                .FirstOrDefaultAsync(t => !t.TransactionType &&   // transactionType - OUT
                                          t.CustomerId == transaction.CustomerId &&
                                          t.ProductId == transaction.ProductId);

            if (lastTransaction != null)
            {
                transaction.TransactionParentId = lastTransaction.TransactionId;
            }
            context.TransactionHistories.Add(transaction);
            await context.SaveChangesAsync();
            return (true, 0);
        }

        public async Task<int> GetRemaingQuantity(TransactionHistory transaction)
        {
            var lastOutTransaction = await context.TransactionHistories
            .OrderByDescending(t => t.TransactionDateTime)
            .FirstOrDefaultAsync(t => !t.TransactionType &&   // transactionType - OUT
                                      t.CustomerId == transaction.CustomerId &&
                                      t.ProductId == transaction.ProductId);

            // Calculate remaining quantity
            int remainingQuantity = 0;
            if (lastOutTransaction != null)
            {
                int totalQuantityIn = await context.TransactionHistories
                .Where(t => t.TransactionParentId == lastOutTransaction.TransactionId)
                .SumAsync(t => t.Quantity);

                remainingQuantity = lastOutTransaction.Quantity - totalQuantityIn;
            }
            return remainingQuantity;
        }

        public async Task<bool> DeleteAllTransactions()
        {
            var allProducts = context.Products.ToList();

            foreach (var product in allProducts)
            {
                product.QuantityTotal += product.QuantityBooked;
                product.QuantityBooked = 0;
            }
            await context.SaveChangesAsync();

            var allRecords = context.TransactionHistories.ToList();
            context.TransactionHistories.RemoveRange(allRecords);
            await context.SaveChangesAsync();

            return true;
        }

    }
}
