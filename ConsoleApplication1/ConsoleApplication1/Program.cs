using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonReadWrite st = new JsonReadWrite();
            if (File.Exists(@"H:\TrackAmazonProduct-master\TrackAmazonProduct-master\ConsoleApplication1\ConsoleApplication1\product.json"))
            {
                Console.WriteLine("Product file exist do you want to create new product File? [y/n]");
                ConsoleKeyInfo cki = Console.ReadKey();

                if (cki.Key.ToString().ToLower().Equals("y"))
                {
                    createNewJson(st);
                }
            }
            else
            {
                createNewJson(st);
            }
  
            object objJson=st.readFile();
            int productCount = ((ConsoleApplication1.RootObject)objJson).product.Count;
            string[] productName = new string[productCount];
            string[] productValue = new string[productCount];

            bool sendMail = false;

            for (int k=0;k<productCount; k++)
            {
                string url = ((ConsoleApplication1.RootObject)objJson).product[k].url;

                HttpClient httpClient = new HttpClient();

                Task<string> html = httpClient.GetStringAsync(url);

                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(html.Result.ToString().Trim());

                string nodeProduct = htmlDocument.DocumentNode.SelectNodes("//span[@id='productTitle']")[0].InnerText.ToString().Trim();

                decimal nodePrice = Decimal.Parse(Regex.Replace(htmlDocument.DocumentNode.SelectNodes("//span[@id='priceblock_ourprice']")[0].InnerText.ToString().Trim().Replace(" ", ""), @"\s+", String.Empty), NumberStyles.Currency);
                
                if (nodePrice <= int.Parse(((ConsoleApplication1.RootObject)objJson).product[k].value))
                {
                    sendMail = true;
                    productName[k] = nodeProduct;
                    productValue[k] = nodePrice.ToString();
                }

            }

            if (sendMail==true)
            {
               SendMail(String.Join(", ", productName),String.Join(", ", productValue));
            }
        }

        public static void createNewJson(JsonReadWrite st)
        {
            Console.WriteLine("\nEnter the number of product you want to track");
            string numProduct = Console.ReadLine();       
            st.GenerateJson(int.Parse(numProduct));
        }
        public static void SendMail(string productName,string newProductPrice)
        {
            try
            {
                string body = "The Price of Products " + productName + " is " + newProductPrice;
                MailAddress fromAddress = new MailAddress("emailaddress", "From Name");
                MailAddress toAddress = new MailAddress("emailaddress", "To Name");
                const string fromPassword = "emailpassword";
                const string subject = "Price Has Been Decreased of Product";
                

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }


                Console.WriteLine("Mail has been send");
            }
            catch (Exception ex)
            {             
                Console.WriteLine(ex);
            }
        }
    }
}
