using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Product
    {
        public string url { get; set; }
        public string value { get; set; }
    }

    public class RootObject
    {
        public List<Product> product { get; set; }
    }
    class JsonReadWrite
    {
        public void GenerateJson(int numProduct)
        {
            List<Product> listProduct = new List<Product>();
            for (int i=0;i<numProduct;i++)
            {
                Console.WriteLine("Enter the product url and min price.");
                string productUrl = Console.ReadLine();
                string productMin = Console.ReadLine();
                Product p1 = new Product();
                p1.url = productUrl;
                p1.value = productMin;
                listProduct.Add(p1);
            }

            RootObject rt = new RootObject();
            rt.product = listProduct;
            string strJson = JsonConvert.SerializeObject(rt);  
            System.IO.File.WriteAllText(@"H:\TrackAmazonProduct-master\TrackAmazonProduct-master\ConsoleApplication1\ConsoleApplication1\product.json", strJson);      
        }    



        public object readFile()
        {
            string jsonStr = File.ReadAllText(@"H:\TrackAmazonProduct-master\TrackAmazonProduct-master\ConsoleApplication1\ConsoleApplication1\product.json");
            RootObject objJson= JsonConvert.DeserializeObject<RootObject>(jsonStr);
            return objJson;
        }
    }
}
