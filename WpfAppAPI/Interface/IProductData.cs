using System.Collections.Generic;
using WebApplication.Entities;

namespace WebApplication.Interface
{
    public interface IProductData
    {
        List<Product> GetProducts();
        Product GetProductById(int id);
        void SaveProduct(Product product);
        void DeleteProduct(int id);
    }
}
