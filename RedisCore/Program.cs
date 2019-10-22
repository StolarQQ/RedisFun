using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ExcelPlayGround;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using RedisCore.PDF;
using StackExchange.Redis;

namespace RedisCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            //await RedisInit();

            //#region PDF

            //var bytePdf = new byte[1];
            //var inStream = new MemoryStream(bytePdf);
            //File.WriteAllBytes("C:\\Users\\Lyns\\Desktop\\Test123.pdf", bytePdf);


            //// Simple check if file is pdf
            //var pdfTool = new PdfTools();
            //var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Console.WriteLine(pdfTool.IsPdf($"{desktopPath}/Test123.pdf"));


            //// Create blank pdf
            //var stream = new MemoryStream();
            //var pdf = new PdfDocument();
            //var page = pdf.AddPage();
            //pdf.Save(stream, false);
            //pdf.Close();
            //var resultPdf = stream.ToArray();

            //// Write pdf
            //File.WriteAllBytes("C:\\Users\\Lyns\\Desktop\\Stolar.pdf", resultPdf);


            //var randombytePdf = new byte[1];
            //File.WriteAllBytes($"{desktopPath}/Rafał.pdf", randombytePdf);
            //File.ReadAllBytes("");


            //var pdfTools = new PdfTools();
            //pdfTools.MergePdfs(new[]
            //{
            //    $"{desktopPath}/xD.pdf",
            //    $"{desktopPath}/ReSharper_DefaultKeymap_VSscheme.pdf"

            //});

            //var fs = new FileStream($"{desktopPath}/xD1.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            //var openpdf = PdfReader.Open(inStream, PdfDocumentOpenMode.Import);

            //#endregion

            var excel = new ExcelService();
            excel.CreateXlsFile();


            Console.ReadKey();
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

            var options = new ConfigurationOptions
            {
                EndPoints = { "ip:6379" },
                Password = "password",
                Ssl = Convert.ToBoolean(ConfigManager.GetAppSettings("isSSL"))
            };

            options.CertificateSelection += OptionsOnCertificateSelection;

            var redis = ConnectionMultiplexer.Connect(options);
            _database = redis.GetDatabase();
        }

        private static X509Certificate OptionsOnCertificateSelection(object s, string t,
            X509CertificateCollection local, X509Certificate remote, string[] a)
        {
            var certPath= ConfigManager.GetAppSettings("certRoute");
            return new X509Certificate2(certPath);
        }

        public async Task<string> GetCache(string key)
            => await _database.StringGetAsync(key);

        public async Task SetCache(string key, string value)
            => await _database.StringSetAsync(key, value);

    }

    public static class ConfigManager
    {
        public static string GetAppSettings(string keyValue) => ConfigurationManager.AppSettings[keyValue];

        public static string GetCustomSection(string section)
        {
            var collection = (NameValueCollection)ConfigurationManager.GetSection("section");
            var value = collection["sectionName"];
            return value;
        }

        internal static byte[] ReadFile(string fileName)
        {
            using (var f = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var size = (int)f.Length;
                byte[] data = new byte[size];
                size = f.Read(data, 0, size);
                f.Close();
                return data;
            }
        }
    }

    public class CertUtils
    {
        public void CertX609()
        {
            var chain = new X509Chain
            {
                ChainPolicy =
                {
                    RevocationMode = X509RevocationMode.NoCheck,
                    RevocationFlag = X509RevocationFlag.ExcludeRoot,
                    VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority,
                    VerificationTime = DateTime.Now,
                    UrlRetrievalTimeout = new TimeSpan(0, 0, 0)
                }
            };
        }
    }
}
