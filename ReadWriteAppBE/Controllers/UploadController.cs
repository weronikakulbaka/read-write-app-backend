using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadWriteAppBE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ReadWriteAppBE.Controllers
{
    // POST api/upload/{id}
    public class UploadController : ApiController
    {
        public Laptops ParseProductToXMLData(Product[] data) {
            var elements = new List<ProductXML>();
            foreach (Product element in data) {
                ProductXML laptop = new ProductXML();
                laptop.screen = new Screen();
                laptop.disc = new Disc();
                laptop.graphic_card = new GraphicCard();
                laptop.processor = new Processor();

                laptop.manufacturer = element.Producent;
                laptop.screen.size = element.Matryca;
                laptop.screen.resolution = element.Rozdzielczosc;
                laptop.screen.type = element.TypMatrycy;
                laptop.screen.@touch = element.DotykowyEkran;
                laptop.processor.name = element.CPU;
                laptop.processor.physical_cores = element.IloscRdzeni;
                laptop.processor.clock_speed = element.MHZ;
                laptop.ram = element.RAM;
                laptop.disc.storage = element.PojemnoscDysku;
                laptop.disc.@type = element.RodzajDysku;
                laptop.graphic_card.name = element.GPU;
                laptop.graphic_card.memory = element.VRAM;
                laptop.os = element.SystemOperacyjny;
                laptop.disc_reader = element.NapedOptyczny;
                elements.Add(laptop);
            }
            Laptops XMLData = new Laptops();
            XMLData.laptops = new Laptop();
            XMLData.laptops.laptop = new List<ProductXML>();
            XMLData.laptops.laptop = elements;
            return XMLData;
        }

        public List<Product> ParseXMLDataToProduct(Laptops data) {
            List<Product> elements = new List<Product>();
            foreach (ProductXML laptop in data.laptops.laptop) 
            {
                  Product element = new Product();
                  element.Producent = laptop.manufacturer;
                  element.Matryca = laptop.screen.size;
                  element.Rozdzielczosc = laptop.screen.resolution;
                  element.TypMatrycy = laptop.screen.type;
                  element.DotykowyEkran = laptop.screen.@touch;
                  element.CPU = laptop.processor.name;
                  element.IloscRdzeni = laptop.processor.physical_cores;
                  element.MHZ = laptop.processor.clock_speed;
                  element.RAM = laptop.ram;
                  element.PojemnoscDysku = laptop.disc.storage;
                  element.RodzajDysku = laptop.disc.@type;
                  element.GPU = laptop.graphic_card.name;
                  element.VRAM = laptop.graphic_card.memory;
                  element.SystemOperacyjny = laptop.os;
                  element.NapedOptyczny = laptop.disc_reader;
                  elements.Add(element);
            }
            return elements;
        }


        [HttpPost]
        public HttpResponseMessage Post(int id, Product[] sendData)
        {
            if (id == 1 && sendData == null)
            {
                var xmlPath = @"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\katalog.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                string json = JsonConvert.SerializeXmlNode(doc);

                Laptops laptops = JsonConvert.DeserializeObject<Laptops>(json);

                List<Product> products = ParseXMLDataToProduct(laptops);


                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(products).ToString(), Encoding.UTF8, "application/json")
                };

            }
            else if (id == 1 && sendData != null)
            {
                Laptops objectToSerialize = ParseProductToXMLData(sendData);
                using (StreamWriter myWriter = new StreamWriter(@"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\zapisane-dane.xml", false))
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(Laptop));
                    mySerializer.Serialize(myWriter, objectToSerialize.laptops);
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(sendData).ToString(), Encoding.UTF8, "application/json")
                };


            }
            else if (id == 2 && sendData == null)
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

                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(products).ToString(), Encoding.UTF8, "application/json")
                };
            }
            else if (id == 2 && sendData != null)
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

                if (sb.ToString().Length > 10)
                {
                    StreamWriter sw = new StreamWriter(@"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\zapisane-dane.txt");
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(sendData).ToString(), Encoding.UTF8, "application/json")
                };
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(null, Encoding.UTF8, "application/json")
            };


        }


    }
}
