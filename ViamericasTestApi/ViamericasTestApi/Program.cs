using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ViamericasTestApi
{
    class Program
    {
        static void Main(string[] args)
        {

            GetToken();
            Console.ReadLine();
        }

        private static void GetToken()
        {
            try
            {

                System.Net.ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls12 ;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender1, certificate, chain, sslPolicyErrors) => true;

                var request = (HttpWebRequest)WebRequest.Create("https://stg-apitest.viamericas.io/v2/viapagos/login");
               
                // Need to pass this through POST Request
                var data = new dataLogin() { companyCode = "T068", username = "payerTestT068", password = "Prueba1234567.",  };
                var jsonParam = JsonConvert.SerializeObject(data);

                var encoding = new UTF8Encoding();
                var bytes = encoding.GetBytes(jsonParam);

                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "application/json";

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }

                
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // grab the response
                        using (var responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                                using (var reader = new StreamReader(responseStream))
                                {
                                    responseValue = reader.ReadToEnd();
                                    Console.WriteLine(responseValue);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
