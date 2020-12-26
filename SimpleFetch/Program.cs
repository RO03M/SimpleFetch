using System.Text.Json;
using System.Net;
using System.IO;
using System.Text;
using System;

namespace SimpleFetch {
    class Program {

        protected string url = "https://www.sypics.com/api/login";
        public string msg = "";
        public int id = 1;

        static void Main(string[] args) {
            Program program = new Program();
            Console.Write("Digite o id: ");
            program.id = Convert.ToInt32(Console.ReadLine());
            program.Fetch(program.url);
        }

        private void Fetch(string url) {
            string bodyData = "this wont work, but whatever";
            var body = Encoding.UTF8.GetBytes(bodyData);
            HttpWebRequest httpRequest = (HttpWebRequest) WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/x-www-form-urlencoded";

            using (Stream passer = httpRequest.GetRequestStream()) {
                passer.Write(body, 0, body.Length);
                passer.Close();
            }

            HttpWebResponse response = (HttpWebResponse) httpRequest.GetResponse();


            using (Stream stream = response.GetResponseStream()) {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                string line;
                string jsonString = "";
                while ((line = reader.ReadLine()) != null) {
                    jsonString += line;
                }
                JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;
                string token = root.GetProperty("data").GetProperty("token").ToString();
                GetProfile(token);
            }
        }

        private void GetProfile(string token) {
            string url = "https://www.sypics.com/api/profile?id=" + id;
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(url);
            http.Method = "GET";
            http.ContentType = "application/x-www-form-urlencoded";
            http.Headers["Authorization"] = token;

            HttpWebResponse request = (HttpWebResponse) http.GetResponse();

            using (Stream stream = request.GetResponseStream()) {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                string line;
                while ((line = reader.ReadLine()) != null) {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
