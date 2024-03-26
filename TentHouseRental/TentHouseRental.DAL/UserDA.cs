using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TentHouseRental.DAL.Models;

namespace TentHouseRental.DAL
{
    public class UserDA : IUserDA
    {
        private readonly TentHouseRentalContext context;
        public UserDA(TentHouseRentalContext context)
        {
            this.context = context;
        }

        public async Task<int> IsUserExist(string email, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                if (user.Password == password)
                {
                    return user.UserId;
                }
                else
                {
                    return 0; // wrong password
                }
            }
            else
            {
                return -1; // user does not exist
            }
        }

    }
}
