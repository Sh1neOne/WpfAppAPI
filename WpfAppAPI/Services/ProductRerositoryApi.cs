using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication.Entities;
using WebApplication.Interface;

namespace WebApplication.Service
{
    public class ProductRerositoryApi : IProductData
    {

        private HttpClient httpClient;
        private string apiuri;

        public ProductRerositoryApi()
        {
            this.httpClient = new HttpClient();
            apiuri = ConfigurationManager.AppSettings.Get("APIURI");
        }

        public List<Product> GetProducts()
        {
            string json = httpClient.GetStringAsync($"{apiuri}/getproducts").Result;
            return JsonConvert.DeserializeObject<List<Product>>(json);
        }

        public Product GetProductById(int id)
        {
            string json = httpClient.GetStringAsync($"{apiuri}/getproduct/{id}").Result;
            return JsonConvert.DeserializeObject<Product>(json);
        }

        public void SaveProduct(Product product)
        {
            var r = httpClient.PostAsync(requestUri: $"{apiuri}/addproduct",
                content: new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8,
                mediaType: "application/json")).Result;     
        }

        public void DeleteProduct(int id)
        {
            var r = httpClient.DeleteAsync(requestUri: $"{apiuri}/deleteproduct/{id}").Result;
        }
    }
}
