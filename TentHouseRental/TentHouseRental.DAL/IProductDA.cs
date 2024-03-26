using TentHouseRental.DAL.Models;

namespace TentHouseRental.DAL
{
    public interface IProductDA
    {
        Task<bool> AddProduct(Product newProduct);
        Task<List<Product>> GetAllProduct();
        Task<Product> GetProductById(int productId);
        Task<bool> RemoveProduct(int productId);
        Task<bool> UpdateProduct(int productId, Product product);
        Task<bool> UpdateProductQuantity(int productId, int transactionQuantity, bool transactionType);
    }
}