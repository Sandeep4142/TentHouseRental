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
    public class ProductDA : IProductDA
    {
        private readonly TentHouseRentalContext context;
        public ProductDA(TentHouseRentalContext context)
        {
            this.context = context;
        }

        public async Task<List<Product>> GetAllProduct()
        {
            List<Product> productList = await context.Products.OrderBy(p => p.ProductTitle).ToListAsync();
            return productList;

        }

        public async Task<Product> GetProductById(int productId)
        {

            Product product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            return product;
        }

        public async Task<bool> AddProduct(Product newProduct)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.ProductTitle == newProduct.ProductTitle);
            if (product != null)
            {
                return false;
            }
            else
            {
                context.Products.Add(newProduct);
                await context.SaveChangesAsync();
                return true;
            }

        }

        public async Task<bool> UpdateProduct(int productId, Product product)
        {
            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                existingProduct.ProductTitle = product.ProductTitle;
                existingProduct.Price = product.Price;
                existingProduct.QuantityTotal = product.QuantityTotal;
                existingProduct.QuantityBooked = product.QuantityBooked;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateProductQuantity(int productId, int transactionQuantity, bool transactionType)
        {
            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                if (transactionType == true)
                {
                    existingProduct.QuantityTotal += transactionQuantity;
                    existingProduct.QuantityBooked -= transactionQuantity;
                }
                else
                {
                    existingProduct.QuantityTotal -= transactionQuantity;
                    existingProduct.QuantityBooked += transactionQuantity;
                }

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveProduct(int productId)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                context.Remove(product);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
