using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentHouseRental.DAL.Models;
using TentHouseRental.Models;

namespace TentHouseRental.DAL
{
    public static class ModelConversion
    {
        public static Customer CustomerModelToCustomerDB(this CustomerModel customer)
        {
            Customer customerDB = new Customer()
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
            };
            return customerDB;

        }

        public static CustomerModel CustomerDBToCustomerModel(this Customer customerDB)
        {
            CustomerModel customer = new CustomerModel()
            {
                CustomerId = customerDB.CustomerId,
                CustomerName = customerDB.CustomerName,
            };
            return customer;

        }

        public static Product ProductModelToProductDB(this ProductModel product)
        {
            Product productDB = new Product()
            {
                ProductId = product.ProductId,
                ProductTitle = product.ProductTitle,
                Price = product.Price,
                QuantityBooked = product.QuantityBooked,
                QuantityTotal = product.QuantityTotal,
            };
            return productDB;
        }

        public static ProductModel ProductDBToProductModel(this Product productDB)
        {
            ProductModel product = new ProductModel()
            {
                ProductId = productDB.ProductId,
                ProductTitle = productDB.ProductTitle,
                Price = productDB.Price,
                QuantityBooked = productDB.QuantityBooked,
                QuantityTotal = productDB.QuantityTotal,
            };
            return product;
        }

        public static User UserModelToUserDB(this UserModel user)
        {
            User userDB = new User()
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
            };
            return userDB;
        }

        public static UserModel UserDBToUserModel(this User userDB)
        {
            UserModel user = new UserModel()
            {
                UserId = userDB.UserId,
                Email = userDB.Email,
                Name = userDB.Name,
                Password = userDB.Password,
            };
            return user;
        }

        public static TransactionHistory TransactionModelToTransactionDB(this TransactionHistoryModel transaction) 
        {
            TransactionHistory transactionDB = new TransactionHistory()
            {
                TransactionId = transaction.TransactionId,
                TransactionType = transaction.TransactionType,
                TransactionDateTime = DateTime.Now,
                TransactionParentId = transaction.TransactionParentId,
                CustomerId = transaction.CustomerId,
                ProductId = transaction.ProductId,
                Quantity = transaction.Quantity,
            };
            return transactionDB;
        }

        public static TransactionHistoryModel TransactionDBToTransactionModel(this TransactionHistory transactionDB)
        {
            TransactionHistoryModel transaction = new TransactionHistoryModel()
            {
                TransactionId = transactionDB.TransactionId,
                TransactionType = transactionDB.TransactionType,
                TransactionDateTime = transactionDB.TransactionDateTime,
                TransactionParentId = transactionDB.TransactionParentId,
                CustomerId = transactionDB.CustomerId,
                ProductId = transactionDB.ProductId,
                Quantity = transactionDB.Quantity,
            };
            return transaction;
        }

    }
}
