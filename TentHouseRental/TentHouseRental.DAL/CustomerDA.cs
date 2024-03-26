using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentHouseRental.DAL.Models;
using TentHouseRental.Utils;

namespace TentHouseRental.DAL
{
    public class CustomerDA : ICustomerDA
    {
        private readonly TentHouseRentalContext context;
        public CustomerDA(TentHouseRentalContext context)
        {
            this.context = context;
        }

        public async Task<List<Customer>> GetAllCustomer()
        {
            List<Customer> customerList = await context.Customers.OrderBy(c => c.CustomerName).ToListAsync();
            return customerList;
        }

        public async Task<bool> AddCustomer(Customer customer)
        {
            var existingCustomer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerName == customer.CustomerName);

            // If the customer with the same name already exists, return false
            if (existingCustomer != null)
            {
                return false;
            }
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
