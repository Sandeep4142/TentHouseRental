using TentHouseRental.Models;

namespace TentHouseRental.BussinessLogic
{
    public interface ITentHouseRentalService
    {
        Task<bool> AddCustomer(CustomerModel customer);
        Task<bool> AddProduct(ProductModel product);
        Task<(bool, int)> AddTransaction(TransactionHistoryModel transaction);
        Task<bool> DeleteAllTransactions();
        Task<List<CustomerModel>> GetAllCustomer();
        Task<List<ProductModel>> GetAllProduct();
        Task<List<TransactionHistoryModel>> GetAllTransaction();
        Task<ProductModel> GetProductById(int productId);
        Task<bool> RemoveProduct(int productId);
        Task<bool> UpdateProduct(int productId, ProductModel product);
        Task<List<InventoryItem>> GetInventoryDetailedReport();
        Task<List<InventoryItem>> GetInventoryDetailedReportByDate(DateTime date);
        Task<List<InventoryItem>> GetInventoryDetailedReportByMonth(DateTime date);
        Task<int> IsUserExist(string email, string password);
    }
}