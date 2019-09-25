using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using StackExchange.Redis;

namespace RedisCore
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //await RedisInit();
            // var fs = new FileStream($"{desktopPath}/xD1.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //var openpdf = PdfReader.Open(inStream, PdfDocumentOpenMode.Import);



            var bytePdf = new byte[1];
            var inStream = new MemoryStream(bytePdf);
            File.WriteAllBytes("C:\\Users\\Lyns\\Desktop\\Test123.pdf", bytePdf);

            // Simple check if file is pdf
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.WriteLine(IsPdf($"{desktopPath}/Test123.pdf"));


            // Create blank pdf
            var stream = new MemoryStream();
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            pdf.Save(stream, false);
            pdf.Close();
            var resultPdf = stream.ToArray();

            // Write pdf
            File.WriteAllBytes("C:\\Users\\Lyns\\Desktop\\Stolar.pdf", resultPdf);






            //File.WriteAllBytes($"{desktopPath}/Rafał.pdf", pdf);

            //File.ReadAllBytes("");


            //var pdfSharper = new PdfSharper();
            //pdfSharper.MergePdfs(new[]
            //{
            //    $"{desktopPath}/xD.pdf",
            //    $"{desktopPath}/ReSharper_DefaultKeymap_VSscheme.pdf"

            //});

            Console.ReadKey();
        }

        static bool IsPdf(string path)
        {
            var pdfString = "%PDF-";
            var pdfBytes = Encoding.ASCII.GetBytes(pdfString);
            var len = pdfBytes.Length;
            var buf = new byte[len];
            var remaining = len;
            var pos = 0;

            using (var f = File.OpenRead(path))
            {
                while (remaining > 0)
                {
                    var amtRead = f.Read(buf, pos, remaining);
                    if (amtRead == 0) return false;
                    remaining -= amtRead;
                    pos += amtRead;
                }
            }
            return pdfBytes.SequenceEqual(buf);
        }

        private static async Task RedisInit()
        {
            var redisService = new Redis();
            await redisService.SetCache("name", "Andrew");
            var data = await redisService.GetCache("name");
            Console.WriteLine($"Fetching data from redis cache '{data}'.");

            // Work with objects .NET

            var employee = new Employee("1", "Micheal", 25);
            await redisService.SetCache("Employee1", JsonConvert.SerializeObject(employee));
            var employeeFromCache = JsonConvert.DeserializeObject<Employee>(await redisService.GetCache("Employee1"));

            Console.WriteLine(employeeFromCache.Id);
            Console.WriteLine(employeeFromCache.Name);
            Console.WriteLine(employeeFromCache.Age);
        }
    }

    public interface IRedis
    {
        Task<string> GetCache(string key);
        Task SetCache(string key, string value);
    }

    public class Redis : IRedis
    {
        private readonly IDatabase _database;

        public Redis()
        {
            // Connect to docker container - default machine win 7 

            var config = ConfigurationManager.AppSettings["redisDB"];
            var redis = ConnectionMultiplexer.Connect(config);
            _database = redis.GetDatabase();
        }

        public async Task<string> GetCache(string key)
            => await _database.StringGetAsync(key);

        public async Task SetCache(string key, string value)
            => await _database.StringSetAsync(key, value);

    }
}
