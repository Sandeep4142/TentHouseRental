
namespace TentHouseRental.DAL
{
    public interface IUserDA
    {
        Task<int> IsUserExist(string email, string password);
    }
}