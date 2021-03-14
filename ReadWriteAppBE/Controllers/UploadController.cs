using CsvHelper;
using Newtonsoft.Json.Linq;
using ReadWriteAppBE.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ReadWriteAppBE.Controllers
{
    public class UploadController : ApiController
    {
        public HttpResponseMessage Post(Product[] sendData)
        {
            var filePath = @"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\katalog.txt";
            string[] lines = File.ReadAllLines(filePath);
           
            List<Product> products = new List<Product>();
            foreach (string line in lines)
            {
                var element = line.Split(';');
                Product product = new Product();
                product.Producent = element[0];
                product.Matryca = element[1];
                product.Rozdzielczosc = element[2];
                product.TypMatrycy = element[3];
                product.DotykowyEkran = element[4];
                product.CPU = element[5];
                product.IloscRdzeni = element[6];
                product.MHZ = element[7];
                product.RAM = element[8];
                product.PojemnoscDysku = element[9];
                product.RodzajDysku = element[10];
                product.GPU = element[11];
                product.VRAM = element[12];
                product.SystemOperacyjny = element[12];
                product.NapedOptyczny = element[12];
                products.Add(product);
            }

            if(sendData != null)
            {
                StringBuilder sb = new StringBuilder("");
                foreach (Product data in sendData)
                {
                    var dataElement = data.Producent + ";" + data.Matryca + ";" 
                        + data.Rozdzielczosc + ";" + data.TypMatrycy + ";" 
                        + data.DotykowyEkran + ";" + data.CPU + ";" 
                        + data.IloscRdzeni + ";" + data.MHZ + ";" 
                        + data.RAM + ";" + data.PojemnoscDysku + ";" 
                        + data.RodzajDysku + ";" + data.GPU + ";" + data.VRAM + ";" + data.SystemOperacyjny + ";" + data.NapedOptyczny + "; \n";
                    sb.Append(dataElement);
                }

                if (sb.ToString().Length > 10) {
                    StreamWriter sw = new StreamWriter(@"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\zapisane-dane.txt");
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                }
            }


            return new HttpResponseMessage()
            {
                Content = new StringContent(JArray.FromObject(products).ToString(), Encoding.UTF8, "application/json")
            };

        }

    }
}
