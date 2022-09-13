using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace testing
{
    internal class Class1
    {
        public static void Main()
        {
            WebRequest request = WebRequest.Create("https://picsum.photos/200");
            Console.WriteLine("\n\nrequest: " + request.ToString());

            WebResponse response = request.GetResponse();
            Console.WriteLine("\n\nresponse: " + response.ToString());

            Stream stream = response.GetResponseStream();
            Console.WriteLine("\n\nStream: " + stream.ToString());
            // Image img = Image.FromStream(stream);
            stream.Close();

            WebHeaderCollection header = response.Headers;
            Console.WriteLine("\n\nheader: " + header.ToString());

            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                Console.WriteLine("\n\nresponse text: " + responseText);
            }

        }
    }
}
