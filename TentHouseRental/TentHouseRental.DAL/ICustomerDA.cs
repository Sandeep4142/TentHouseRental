using TentHouseRental.DAL.Models;

namespace TentHouseRental.DAL
{
    public interface ICustomerDA
    {
        Task<bool> AddCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomer();
    }
}