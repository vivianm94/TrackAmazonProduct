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

        //public void readFile()
        //{
        //    string Json = File.ReadAllText(@"H:\TrackAmazonProduct-master\TrackAmazonProduct-master\ConsoleApplication1\ConsoleApplication1\json1.json");
        //    var dict = JsonConvert.DeserializeObject<IDictionary>(Json);

        //    foreach (var newdic in dict)
        //    {
        //        var newd = newdic;
        //        var key = ((System.Collections.DictionaryEntry)newd).Key;

        //        if (key.Equals("medications"))
        //        {
        //            var newdn = ((System.Collections.DictionaryEntry)newd).Value;
        //            JArray Jary = JArray.Parse(newdn.ToString());
        //            foreach (JObject newdic12 in Jary)
        //            {
        //                var ne = newdic12;

        //                foreach (var rea in ne.Children())
        //                {
        //                    var newrea = rea;

        //                    if (((Newtonsoft.Json.Linq.JProperty)newrea).Name.Equals("betaBlocker"))
        //                    {
        //                        var newwe = newrea.Values().Children().ToArray();

        //                        foreach (var newa12 in newwe)
        //                        {
        //                            var property = ((Newtonsoft.Json.Linq.JProperty)(newa12)).Name;

        //                            if(property.Equals("dose"))
        //                            {
        //                                ((Newtonsoft.Json.Linq.JProperty)newa12).Value = "asdf";
        //                            }
        //                        }
                               
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        public object readFile()
        {
            string jsonStr = File.ReadAllText(@"H:\TrackAmazonProduct-master\TrackAmazonProduct-master\ConsoleApplication1\ConsoleApplication1\product.json");
            RootObject objJson= JsonConvert.DeserializeObject<RootObject>(jsonStr);
            return objJson;
        }
    }
}
