using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadWriteAppBE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
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
        private const string connectionString = @"Server=DESKTOP-46FHMCU;Database=laptopy;User Id=loginTest;Password=1234;";

        string sampleTXT = @"Dell;12"";;matowa;nie;intel i7;4;2800;8GB;240GB;SSD;intel HD Graphics 4000;1GB;Windows 7 Home;;
Asus;14"";1600x900;matowa;nie;intel i5;4;;16GB;120GB;SSD;intel HD Graphics 5000;1GB;;brak;
Fujitsu;14"";1920x1080;blyszczaca;tak;intel i7;8;1900;24GB;500GB;HDD;intel HD Graphics 520;1GB;brak systemu;Blu-Ray;
Huawei;13"";;matowa;nie;intel i7;4;2400;12GB;24GB;HDD;NVIDIA GeForce GTX 1050;;;brak;
MSI;17"";1600x900;blyszczaca;tak;intel i7;4;3300;8GB;60GB;SSD;AMD Radeon Pro 455;1GB;Windows 8.1 Profesional;DVD;
Dell;;1280x800;matowa;nie;intel i7;4;2800;8GB;240GB;SSD;;;Windows 7 Home;brak;
Asus;14"";1600x900;matowa;nie;intel i5;4;2800;;120GB;SSD;intel HD Graphics 5000;1GB;Windows 10 Home;;
Fujitsu;15"";1920x1080;blyszczaca;tak;intel i7;8;2800;24GB;500GB;HDD;intel HD Graphics 520;;brak systemu;Blu-Ray;
Samsung;13"";1366x768;matowa;nie;intel i7;4;2800;12GB;24GB;HDD;NVIDIA GeForce GTX 1050;1GB;Windows 10 Home;brak;
Sony;16"";;blyszczaca;tak;intel i7;4;2800;8GB;;;AMD Radeon Pro 455;1GB;Windows 7 Profesional;DVD;
Samsung;12"";1280x800;matowa;nie;intel i7;;2120;;;;intel HD Graphics 4000;1GB;;brak;
Samsung;14"";1600x900;matowa;nie;intel i5;;;;;SSD;intel HD Graphics 5000;1GB;Windows 10 Home;brak;
Fujitsu;15"";1920x1080;blyszczaca;tak;intel i7;8;2800;24GB;500GB;HDD;intel HD Graphics 520;;brak systemu;Blu-Ray;
Huawei;13"";1366x768;matowa;nie;intel i7;4;3000;;24GB;HDD;NVIDIA GeForce GTX 1050;;Windows 10 Home;brak;
MSI;17"";1600x900;blyszczaca;tak;intel i7;4;9999;8GB;60GB;SSD;AMD Radeon Pro 455;1GB;Windows 7 Profesional;;
Huawei;14"";;matowa;nie;intel i7;4;2200;8GB;16GB;HDD;NVIDIA GeForce GTX 1080;;;brak;
MSI;17"";1600x900;blyszczaca;tak;intel i7;4;3300;8GB;60GB;SSD;AMD Radeon Pro 455;1GB;;;
Asus;;1600x900;blyszczaca;tak;intel i5;2;3200;16GB;320GB;HDD;;;Windows 7 Home;brak;
Asus;14"";1600x900;matowa;nie;intel i5;4;2800;;120GB;SSD;intel HD Graphics 5000;1GB;Windows 10 Profesional;;
Fujitsu;14"";1280x800;blyszczaca;tak;intel i7;8;2800;24GB;500GB;HDD;intel HD Graphics 520;;brak systemu;Blu-Ray;
Samsung;12"";1600x900;;nie;intel i5;4;2800;12GB;24GB;HDD;NVIDIA GeForce GTX 1050;1GB;Windows 8.1 Home;brak;
Sony;11"";;blyszczaca;tak;intel i7;4;2800;8GB;;;AMD Radeon Pro 455;1GB;Windows 7 Profesional;brak;
Samsung;13"";1366x768;;nie;intel i5;;2120;;;;intel HD Graphics 4000;2GB;;DVD;
Samsung;15"";1920x1080;matowa;nie;intel i9;;;;;SSD;intel HD Graphics 4000;2GB;Windows 10 Profesional;Blu-Ray;";
        string sampleXML = @"
<laptops moddate=""2015-10-30 T 10:15"">
  <laptop id=""1"">
    <manufacturer>Asus</manufacturer>
    <screen touch=""no"">
      <size>12""</size>
      <resolution>1600x900</resolution>
      <type>matowy</type>
    </screen>
    <processor>
      <name>i7</name>
      <physical_cores>8</physical_cores>
      <clock_speed>3200</clock_speed>
    </processor>
    <ram>8GB</ram>
    <disc type=""SSD"">
      <storage>240GB</storage>
    </disc>
    <graphic_card>
      <name>intel HD Graphics 4000</name>
      <memory>1GB</memory>
    </graphic_card>
    <os>Windows 7 Home</os>
    <disc_reader>Blu-Ray</disc_reader>
  </laptop>
  <laptop id=""2"">
    <manufacturer>Dell</manufacturer>
    <screen touch=""yes"">
      <size>16""</size>
      <resolution>1376x768</resolution>
      <type/>
    </screen>
    <processor>
      <name>i5</name>
      <physical_cores>4</physical_cores>
      <clock_speed/>
    </processor>
    <ram>16GB</ram>
    <disc>
      <storage>120GB</storage>
    </disc>
    <graphic_card>
      <name>intel HD Graphics 5000</name>
      <memory>2GB</memory>
    </graphic_card>
    <os>Windows Vista</os>
    <disc_reader>brak</disc_reader>
  </laptop>
</laptops>
";

        public Laptops ParseProductToXMLData(Product[] data)
        {
            var elements = new List<ProductXML>();
            foreach (Product element in data)
            {
                ProductXML laptop = new ProductXML();
                laptop.screen = new Screen();
                laptop.disc = new Disc();
                laptop.graphic_card = new GraphicCard();
                laptop.processor = new Processor();

                laptop.@id = element.Id;
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

        public List<Product> ParseXMLDataToProduct(Laptops data)
        {
            List<Product> elements = new List<Product>();
            foreach (ProductXML laptop in data.laptops.laptop)
            {
                Product element = new Product();
                element.Id = laptop.@id;
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
            int i = 1;
            if (id == 1 && sendData == null)
            {
                var xmlPath = @"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\katalog.xml";
                if (!File.Exists(xmlPath))
                {
                    var file = File.Create(xmlPath);
                    StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(sampleXML);
                    sw.Close();
                    file.Close();
                }
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
                if (!File.Exists(filePath))
                {
                    var file = File.Create(filePath);
                    StreamWriter sw = new StreamWriter(file);
                    sw.WriteLine(sampleTXT);
                    sw.Close();
                    file.Close();
                }
                string[] lines = File.ReadAllLines(filePath);

                List<Product> products = new List<Product>();
                foreach (string line in lines)
                {
                    var element = line.Split(';');
                    Product product = new Product();
                    product.Id = i++;
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

            //zapis do bazy
            else if (id == 3 && sendData != null)
            {
                DataTable tvp = new DataTable();
                tvp.Columns.Add(new DataColumn("producent", typeof(string)));
                tvp.Columns.Add(new DataColumn("matryca", typeof(string)));
                tvp.Columns.Add(new DataColumn("rozdzielczosc", typeof(string)));
                tvp.Columns.Add(new DataColumn("typ_matrycy", typeof(string)));
                tvp.Columns.Add(new DataColumn("dotykowy_ekran", typeof(string)));
                tvp.Columns.Add(new DataColumn("cpu", typeof(string)));
                tvp.Columns.Add(new DataColumn("ilosc_rdzeni", typeof(string)));
                tvp.Columns.Add(new DataColumn("mhz", typeof(string)));
                tvp.Columns.Add(new DataColumn("ram", typeof(string)));
                tvp.Columns.Add(new DataColumn("pojemnosc_dysku", typeof(string)));
                tvp.Columns.Add(new DataColumn("rodzaj_dysku", typeof(string)));
                tvp.Columns.Add(new DataColumn("gpu", typeof(string)));
                tvp.Columns.Add(new DataColumn("vram", typeof(string)));
                tvp.Columns.Add(new DataColumn("system_operacyjny", typeof(string)));
                tvp.Columns.Add(new DataColumn("naped_optyczny", typeof(string)));

                foreach (var data in sendData)
                {
                    tvp.Rows.Add(data.Producent, data.Matryca, data.Rozdzielczosc, data.TypMatrycy, data.DotykowyEkran, data.CPU, data.IloscRdzeni,
                                 data.MHZ, data.RAM, data.PojemnoscDysku, data.RodzajDysku, data.GPU, data.VRAM, data.SystemOperacyjny, data.NapedOptyczny);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("dbo.zapiszLaptopy", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter tvparam = cmd.Parameters.AddWithValue("@lista", tvp);
                    // these next lines are important to map the C# DataTable object to the correct SQL User Defined Type
                    tvparam.SqlDbType = SqlDbType.Structured;
                    tvparam.TypeName = "dbo.laptop";

                    var result = cmd.ExecuteReader();
                    connection.Close();
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(sendData).ToString(), Encoding.UTF8, "application/json")
                };
            }

            //odczyt z bazy
            else if (id == 3 && sendData == null)
            {
                List<Product> products = new List<Product>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("dbo.pobierzLaptopy", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var result = cmd.ExecuteReader();

                    while (result.Read())
                    {
                        Product product = new Product();
                        product.Id = int.Parse(result["id"].ToString());
                        product.Producent = result["producent"].ToString();
                        product.Matryca = result["matryca"].ToString();
                        product.Rozdzielczosc = result["rozdzielczosc"].ToString();
                        product.TypMatrycy = result["typ_matrycy"].ToString();
                        product.DotykowyEkran = result["dotykowy_ekran"].ToString();
                        product.CPU = result["cpu"].ToString();
                        product.IloscRdzeni = result["ilosc_rdzeni"].ToString();
                        product.MHZ = result["mhz"].ToString();
                        product.RAM = result["ram"].ToString();
                        product.PojemnoscDysku = result["pojemnosc_dysku"].ToString();
                        product.RodzajDysku = result["rodzaj_dysku"].ToString();
                        product.GPU = result["gpu"].ToString();
                        product.VRAM = result["vram"].ToString();
                        product.SystemOperacyjny = result["system_operacyjny"].ToString();
                        product.NapedOptyczny = result["naped_optyczny"].ToString();
                        products.Add(product);
                    }

                    connection.Close();
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(products).ToString(), Encoding.UTF8, "application/json")
                };
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(null, Encoding.UTF8, "application/json")
            };
        }



    }
}
