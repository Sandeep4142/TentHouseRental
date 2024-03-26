using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentHouseRental.DAL;
using TentHouseRental.DAL.Models;
using TentHouseRental.Models;

namespace TentHouseRental.BussinessLogic
{
    public class TentHouseRentalService : ITentHouseRentalService
    {
        private readonly IProductDA productDA;
        private readonly ITransactionHistoryDA transactionDA;
        private readonly ICustomerDA customerDA;
        private readonly IUserDA userDA;

        public TentHouseRentalService(IProductDA productDA, ITransactionHistoryDA transactionDA, ICustomerDA customerDA, IUserDA userDA)
        {
            this.productDA = productDA;
            this.transactionDA = transactionDA;
            this.customerDA = customerDA;
            this.userDA = userDA;
        }

        public async Task<List<ProductModel>> GetAllProduct()
        {
            List<ProductModel> productList = new List<ProductModel>();
            var products = await productDA.GetAllProduct();
            foreach (var product in products)
            {
                ProductModel prod = product.ProductDBToProductModel();
                productList.Add(prod);
            }
            return productList;
        }

        public async Task<ProductModel> GetProductById(int productId)
        {
            Product productDB = await productDA.GetProductById(productId);
            ProductModel product = productDB.ProductDBToProductModel();
            return product;
        }

        public async Task<bool> AddProduct(ProductModel product)
        {
            Product productDB = product.ProductModelToProductDB();
            return await productDA.AddProduct(productDB);
        }

        public async Task<bool> UpdateProduct(int productId, ProductModel product)
        {
            Product productDB = product.ProductModelToProductDB();
            return await productDA.UpdateProduct(productId, productDB);
        }

        public async Task<bool> RemoveProduct(int productId)
        {
            return await productDA.RemoveProduct(productId);
        }

        public async Task<List<TransactionHistoryModel>> GetAllTransaction()
        {
            List<TransactionHistoryModel> transactionlist = new List<TransactionHistoryModel>();
            var transactions = await transactionDA.GetAllTransaction();
            foreach (var transaction in transactions)
            {
                TransactionHistoryModel tr = transaction.TransactionDBToTransactionModel();
                transactionlist.Add(tr);
            }
            return transactionlist;
        }

        public async Task<(bool, int)> AddTransaction(TransactionHistoryModel transaction)
        {
            TransactionHistory transactionDB = transaction.TransactionModelToTransactionDB();

            if (transaction.TransactionType == false) // for transactionType - OUT
            {
                int remainingQuantity = await transactionDA.GetRemaingQuantity(transactionDB);
                if (remainingQuantity > 0)
                {
                    return (false, remainingQuantity);
                }
            }

            else // for transactionType - IN
            {
                int remainingQuantity = await transactionDA.GetRemaingQuantity(transactionDB);
                if (remainingQuantity == 0)
                {
                    return (false, 0);
                }
                else if (transaction.Quantity > remainingQuantity)
                {
                    return (false, remainingQuantity);
                }
            }

            int productId = transaction.ProductId;
            bool transactionType = transaction.TransactionType;
            int transactionQuantity = transaction.Quantity;
            await productDA.UpdateProductQuantity(productId, transactionQuantity, transactionType);
            return await transactionDA.AddTransaction(transactionDB);
        }

        public async Task<List<CustomerModel>> GetAllCustomer()
        {
            List<CustomerModel> customerList = new List<CustomerModel>();
            var customers = await customerDA.GetAllCustomer();
            foreach (var customer in customers)
            {
                CustomerModel cust = customer.CustomerDBToCustomerModel();
                customerList.Add(cust);
            }
            return customerList;
        }

        public async Task<bool> AddCustomer(CustomerModel customer)
        {
            Customer customerDB = customer.CustomerModelToCustomerDB();
            return await customerDA.AddCustomer(customerDB);
        }

        public async Task<bool> DeleteAllTransactions()
        {
            return await transactionDA.DeleteAllTransactions();
        }

        public async Task<List<InventoryItem>> GetInventoryDetailedReport()
        {
            // Retrieve all products and transactions
            List<ProductModel> productList = new List<ProductModel>();
            var products = await productDA.GetAllProduct();
            foreach (var product in products)
            {
                ProductModel prod = product.ProductDBToProductModel();
                productList.Add(prod);
            }

            List<TransactionHistoryModel> transactionList = new List<TransactionHistoryModel>();
            var transactions = await transactionDA.GetAllTransaction();
            foreach (var transaction in transactions)
            {
                TransactionHistoryModel tr = transaction.TransactionDBToTransactionModel();
                transactionList.Add(tr);
            }

            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            foreach (var product in productList)
            {
                // Filter transactions for the current product
                var productTransactions = transactionList.Where(t => t.ProductId == product.ProductId).ToList();

                int availableQuantity = product.QuantityTotal - productTransactions
                    .Where(t => t.TransactionType) 
                    .Sum(t => t.Quantity);

                InventoryItem inventoryItem = new InventoryItem
                {
                    ItemName = product.ProductTitle,
                    AvailableQuantity = availableQuantity,
                    Transactions = productTransactions
                };

                inventoryItems.Add(inventoryItem);
            }
            return inventoryItems;
        }

        public async Task<List<InventoryItem>> GetInventoryDetailedReportByDate(DateTime selectedDate)
        {
            List<ProductModel> productList = new List<ProductModel>();
            var products = await productDA.GetAllProduct();
            foreach (var product in products)
            {
                ProductModel prod = product.ProductDBToProductModel();
                productList.Add(prod);
            }

            List<TransactionHistoryModel> transactionList = new List<TransactionHistoryModel>();
            var transactions = await transactionDA.GetAllTransaction();
            foreach (var transaction in transactions)
            {
                TransactionHistoryModel tr = transaction.TransactionDBToTransactionModel();
                transactionList.Add(tr);
            }

            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            foreach (var product in productList)
            {
                // Filter transactions for the current product up to the specified date
                var productTransactions = transactionList
                    .Where(t => t.ProductId == product.ProductId && t.TransactionDateTime.Date <= selectedDate.Date)
                    .ToList();

                int totalInQuantity = productTransactions
                    .Where(t => t.TransactionType) 
                    .Sum(t => t.Quantity);

                int totalOutQuantity = productTransactions
                    .Where(t => !t.TransactionType) 
                    .Sum(t => t.Quantity);

                int availableQuantity = product.QuantityTotal+product.QuantityBooked - totalOutQuantity + totalInQuantity;

                InventoryItem inventoryItem = new InventoryItem
                {
                    ItemName = product.ProductTitle,
                    AvailableQuantity = availableQuantity,
                    Transactions = productTransactions
                };

                inventoryItems.Add(inventoryItem);
            }
            return inventoryItems;
        }

        public async Task<List<InventoryItem>> GetInventoryDetailedReportByMonth(DateTime date)
        {
            // start and end dates for the specified month
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1); // Last day of the month

            List<ProductModel> productList = new List<ProductModel>();
            var products = await productDA.GetAllProduct();
            foreach (var product in products)
            {
                ProductModel prod = product.ProductDBToProductModel();
                productList.Add(prod);
            }

            List<TransactionHistoryModel> transactionList = new List<TransactionHistoryModel>();
            var transactions = await transactionDA.GetAllTransaction();
            foreach (var transaction in transactions)
            {
                TransactionHistoryModel tr = transaction.TransactionDBToTransactionModel();
                transactionList.Add(tr);
            }

            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            foreach (var product in productList)
            {
                // Filter transactions for the current product within the specified month
                var productTransactions = transactionList
                    .Where(t => t.ProductId == product.ProductId && t.TransactionDateTime.Date >= startDate.Date && t.TransactionDateTime.Date <= endDate.Date)
                    .ToList();

                int totalInQuantity = productTransactions
                    .Where(t => t.TransactionType) 
                    .Sum(t => t.Quantity);

                int totalOutQuantity = productTransactions
                    .Where(t => !t.TransactionType) 
                    .Sum(t => t.Quantity);

                int availableQuantity = product.QuantityTotal + product.QuantityBooked - totalOutQuantity + totalInQuantity;
                InventoryItem inventoryItem = new InventoryItem
                {
                    ItemName = product.ProductTitle,
                    AvailableQuantity = availableQuantity,
                    Transactions = productTransactions
                };

                inventoryItems.Add(inventoryItem);
            }
            return inventoryItems;
        }

        public async Task<int> IsUserExist(string email, string password)
        {
            return await userDA.IsUserExist(email, password);
        }

    }
}
