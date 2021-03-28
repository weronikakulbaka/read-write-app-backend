using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ReadWriteAppBE.Models
{
    public class Product
    {
        public string Producent { get; set; }
        public string Matryca { get; set; }
        public string Rozdzielczosc { get; set; }
        public string TypMatrycy { get; set; }
        public string DotykowyEkran { get; set; }
        public string CPU { get; set; }
        public string IloscRdzeni { get; set; }
        public string MHZ { get; set; }
        public string RAM { get; set; }
        public string PojemnoscDysku { get; set; }
        public string RodzajDysku { get; set; }
        public string GPU { get; set; }
        public string VRAM { get; set; }
        public string SystemOperacyjny { get; set; }
        public string NapedOptyczny { get; set; }
    }

    [XmlType("disc")]
    public class Disc
    {
       public string @type { get; set; }
       public string storage { get; set; }
    }

    [XmlType("graphic_card")]
    public class GraphicCard
    {
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("memory")]
        public string memory { get; set; }
    }

    [XmlType("processor")]
    public class Processor
    {
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("physical_cores")]
        public string physical_cores { get; set; }
        [XmlElement("clock_speed")]
        public string clock_speed { get; set; }
    }

    [XmlType("screen")]
    public class Screen
    {
        [XmlAttribute("touch")]
        public string @touch { get; set; }
        [XmlElement("size")]
        public string size { get; set; }
        [XmlElement("resolution")]
        public string resolution { get; set; }
        [XmlElement("type")]
        public string type { get; set; }
    }
    [XmlType("laptop")]
    public class ProductXML
    {
        [XmlAttribute("id")]
        public int @id { get; set; }
        [XmlElement("disc")]
        public Disc disc { get; set; }
        [XmlElement("disc_reader")]
        public string disc_reader { get; set; }
        [XmlElement("graphic_card")]
        public GraphicCard graphic_card { get; set; }
        [XmlElement("manufacturer")]
        public string manufacturer { get; set; }
        [XmlElement("os")]
        public string os { get; set; }
        [XmlElement("processor")]
        public Processor processor { get; set; }
        [XmlElement("ram")]
        public string ram { get; set; }
        [XmlElement("screen")]
        public Screen screen { get; set; }
    }

    [Serializable, XmlRoot("laptops"), XmlType("laptops")]
    public class Laptop
    {
        [XmlElement("laptop")]
        public List<ProductXML> laptop { get; set; }
    }

   
    public class Laptops
    {
        [XmlElement("laptops")]
        public Laptop laptops { get; set; }
    }

}