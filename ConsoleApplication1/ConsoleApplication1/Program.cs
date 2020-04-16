using System;
using System.Collections.Generic;
using System.Globalization;
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
            // string url = "https://www.amazon.in/GENERIC-Ultra-Mini-Bluetooth-Dongle-Adapter/dp/B0117H7GZ6/ref=sr_1_14?crid=36FN332QCAZBD&dchild=1&keywords=earphones+under+200+rupees&qid=1586759890&sprefix=earp%2Caps%2C346&sr=8-14";

            string url = "https://www.amazon.in/iVoltaa-Micro-USB-Type-Adapter/dp/B075F927V2/ref=lp_1389401031_1_4?s=electronics&ie=UTF8&qid=1586778682&sr=1-4";

            HttpClient httpClient = new HttpClient();

            var html = httpClient.GetStringAsync(url);

            // Console.WriteLine(html.Result);

            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html.Result.ToString().Trim());

            var nodeProduct = htmlDocument.DocumentNode.SelectNodes("//span[@id='productTitle']");

            var nodePrice= htmlDocument.DocumentNode.SelectNodes("//span[@id='priceblock_ourprice']");

            string ProductName = nodeProduct[0].InnerText.ToString().Trim();

            var ProductPrice = nodePrice[0].InnerText.ToString().Trim().Replace(" ", "");

            var newProductPrice = Decimal.Parse(Regex.Replace(ProductPrice, @"\s+", String.Empty), NumberStyles.Currency);

            Console.WriteLine("ProductName:"+ ProductName+" Price: "+ newProductPrice);

            if (newProductPrice<179)
            {
                sendMail(ProductName, newProductPrice);
            }

        }


        public static void sendMail(string ProductName,decimal newProductPrice)
        {
            try
            {
                var fromAddress = new MailAddress("emailaddress", "From Name");
                var toAddress = new MailAddress("emailaddress", "To Name");
                const string fromPassword = "emailpassword";
                const string subject = "Price Has Been Decreased of Product";
                string body = "The Price of Product" + ProductName + "is" + newProductPrice;

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


                Console.WriteLine("mail Send");
            }
            catch (Exception ex)
            {             
                Console.WriteLine(ex);
            }
        }
    }
}
