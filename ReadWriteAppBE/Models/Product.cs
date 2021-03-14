using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}