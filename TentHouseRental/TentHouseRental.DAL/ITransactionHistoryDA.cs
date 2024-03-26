using TentHouseRental.DAL.Models;

namespace TentHouseRental.DAL
{
    public interface ITransactionHistoryDA
    {
        Task<(bool, int)> AddTransaction(TransactionHistory transaction);
        Task<bool> DeleteAllTransactions();
        Task<List<TransactionHistory>> GetAllTransaction();
        Task<int> GetRemaingQuantity(TransactionHistory transaction);
    }
}