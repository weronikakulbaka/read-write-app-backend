using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace ReadWriteAppBE.Controllers
{
    public class UploadController : ApiController
    {
        public class Product
        {
            public string Producent { get; set; }
            public string Matryca { get; set; }
            public string Rozdzielczosc { get; set; }
            public string TypMatrycy { get; set; }
            public string LiczbaRdzeniFizycznych { get; set; }
            public string Taktowanie { get; set; }
            public string RAM { get; set; }
            public string PojemnoscDysku { get; set; }
            public string TypDysku { get; set; }
            public string KartaGraficzna { get; set; }
            public string PamiecKartyGraficznej { get; set; }
            public string SystemOperacyjny { get; set; }
            public string NapedOptyczny { get; set; }
        }

        public HttpResponseMessage Post(HttpRequestMessage changedProducts)
        {
            var filePath = @"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\katalog.txt";
            string[] lines = File.ReadAllLines(filePath);
           
            List<Product> products = new List<Product>();
            var valueFromClient = changedProducts.Content.ReadAsStringAsync().Result;

            //using (StreamWriter file = File.CreateText(@"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\test.txt"))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    //serialize object directly into file stream
            //    serializer.Serialize(file, valueFromClient);
            //}

            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                var element = line.Split(';');

                Product product = new Product();
                product.Producent = element[0];
                product.Matryca = element[1];
                product.Rozdzielczosc = element[2];
                product.TypMatrycy = element[3];
                product.LiczbaRdzeniFizycznych = element[4];
                product.Taktowanie = element[5];
                product.RAM = element[6];
                product.PojemnoscDysku = element[7];
                product.TypDysku = element[8];
                product.KartaGraficzna = element[9];
                product.PamiecKartyGraficznej = element[10];
                product.SystemOperacyjny = element[11];
                product.NapedOptyczny = element[12];
                products.Add(product);
                Console.WriteLine(products);
            }



            if (valueFromClient.Length > 0) {
                StreamWriter sw = new StreamWriter(@"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\test.txt");
                sw.WriteLine(valueFromClient);
                sw.Close();
            }

            //var reader = new StreamReader(filePath);
            //var data = reader.ReadToEnd();
            //var dataBinary = Encoding.UTF8.GetBytes(data);
            //var txtStream = new MemoryStream(dataBinary);

            // create the response and returns it
            //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            //result.Content = new StreamContent(txtStream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = "katalog.txt"
            //};

            //return result;

            return new HttpResponseMessage()
            {
                Content = new StringContent(JArray.FromObject(products).ToString(), Encoding.UTF8, "application/json")
            };

        }

    }
}
