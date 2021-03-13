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
        public class MyModel
        {
            public string Key { get; set; }
        }

        public HttpResponseMessage Post(HttpRequestMessage value)
        {
            var filePath = @"E:\STUDIA_III_SEMESTR\IntegracjaSystemowLab\Lab2\ReadWriteAppBE\ReadWriteAppBE\Resources\katalog.txt";
            var valueFromClient = value.Content.ReadAsStringAsync().Result;

            if (valueFromClient.Length > 0) {
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine(valueFromClient);
                sw.Close();
            }
            
            var reader = new StreamReader(filePath);
            var data = reader.ReadToEnd();
            var dataBinary = Encoding.UTF8.GetBytes(data);
            var txtStream = new MemoryStream(dataBinary);

            // create the response and returns it
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(txtStream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "katalog.txt"
            };

            return result;

        }

    }
}
