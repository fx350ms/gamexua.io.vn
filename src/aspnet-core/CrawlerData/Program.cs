using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using NLog;
using RestSharp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CrawlerData
{
    internal class Program
    {

        static ApplicationDbContext context = new ApplicationDbContext();
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        static async Task Main(string[] args)
        {
            //
            var config = new NLog.Config.XmlLoggingConfiguration("NLog.config");
            LogManager.Configuration = config;
            Logger.Info("Application is starting...");

            for (int i = 1; i <= 15; i++)
            {

                string url = "https://www.playemulator.io/nes-online/"; // URL của trang web bạn muốn lấy dữ liệu
                if (i > 1)
                {
                    url += "page-" + i;
                }
                Logger.Info($"FetchGameData: {url}");
                await FetchGameData(url);

            }

            Console.ReadLine();
        }


        static async Task FetchGameData(string websiteUrl)
        {
            try
            {
                using HttpClient client = new HttpClient();

                // Tải nội dung HTML của trang chính
                string html = await client.GetStringAsync(websiteUrl);

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Tìm thẻ ul có class 'emu-list'
                var ulNode = htmlDoc.DocumentNode.SelectSingleNode("//ul[contains(@class, 'emu-list')]");
                if (ulNode == null)
                {
                    Console.WriteLine("No <ul> element with class 'emu-list' found.");
                    return;
                }

                // Lấy danh sách thẻ li
                var liNodes = ulNode.SelectNodes("./li");
                if (liNodes == null)
                {
                    Console.WriteLine("No <li> elements found.");
                    return;
                }

                foreach (var liNode in liNodes)
                {
                    // Lấy href của thẻ a
                    var aNode = liNode.SelectSingleNode(".//a");
                    string linkHref = aNode?.GetAttributeValue("href", "No href found") ?? "No href found";

                    if (!string.IsNullOrEmpty(linkHref) && linkHref != "No href found")
                    {
                        Console.WriteLine($"Page link: {linkHref}");

                        // Tải nội dung HTML từ trang liên kết
                        string pageHtml = await client.GetStringAsync(linkHref);
                        HtmlDocument pageDoc = new HtmlDocument();
                        pageDoc.LoadHtml(pageHtml);

                        // Tìm thẻ input hidden có name='game_id'
                        var inputNode = pageDoc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @name='game_id']");

                        string name = pageDoc.DocumentNode.SelectSingleNode("//h1[@class='h3 mb-2' and @itemprop='name']").InnerText;

                        if (inputNode != null)
                        {
                            string gameId = inputNode.GetAttributeValue("value", "No game_id found");
                            Console.WriteLine($"Game ID: {gameId} - {name}");

                            // Gửi yêu cầu HTTP POST đến API với game_id
                            await PostGameId(gameId, name);
                        }
                        else
                        {
                            Console.WriteLine("No game_id found on the page.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task PostGameId(string gameId, string name)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest("https://www.playemulator.io/emu/", Method.Post);
                request.AddHeader("authority", "www.playemulator.io");
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.AddHeader("accept-language", "vi,en-US;q=0.9,en;q=0.8");
                request.AddHeader("cache-control", "max-age=0");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                request.AddParameter("game_id", gameId);
                RestResponse response = await client.ExecuteAsync(request);

                ExtractRomAndImage(name, response.Content);
            }

            catch (Exception ex)
            {
                Logger.Error($"Error while posting game_id: {gameId}, name: {name}");
                Console.WriteLine($"Error while posting game_id: {ex.Message}");
            }
        }


        static async void ExtractRomAndImage(string name, string html)
        {
            try
            {
                // Tải HTML vào HtmlAgilityPack
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Tìm thẻ <script> chứa RetroLoaderConfig
                var scriptNode = htmlDoc.DocumentNode.SelectSingleNode("//script");

                if (scriptNode != null)
                {
                    // Nội dung của thẻ <script>
                    string scriptContent = scriptNode.InnerText.Trim().Replace(" ", "").Replace("\r\n", "").Replace("\t", "");

                    // Tìm giá trị của trường rom

                    var lines = scriptContent.Split(",");

                    // In ra kết quả

                    var rom = lines[2].Split(new char[] { '"' })[1];
                    var image = lines[3].Split(new char[] { '"' })[1];

                    AddGameToDatabase(name, image, rom);

                    Console.WriteLine($"ROM: {rom}");
                    Console.WriteLine($"Image: {image}");
                }
                else
                {
                    Console.WriteLine("No script with RetroLoaderConfig found.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"ExtractRomAndImage | {name}");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async void AddGameToDatabase(string name, string image, string rom)
        {
            try
            {
                byte[] imageData = await DownloadDataAsync(image);
                byte[] romData = await DownloadDataAsync(rom);


                var game = new Game
                {
                    Name = name,
                    Thumbnail = imageData,
                    Data = romData,
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                };

                using (var context = new ApplicationDbContext())
                {
                    context.Games.Add(game);
                    await context.SaveChangesAsync(); // Sử dụng SaveChangesAsync để đảm bảo đồng bộ
                }

                // Thêm vào bảng Games
                //context.Games.Add(game);
                //context.SaveChanges();
                Logger.Info($"Game added to the database successfully. | {name}");
                Console.WriteLine("Game added to the database successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"AddGameToDatabase | {name}");
            }
            

        }


        static async Task<byte[]> DownloadDataAsync(string url)
        {
            try
            {
                if (url.StartsWith("//"))
                {
                    url = "https:" + url;
                }
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetByteArrayAsync(url);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"DownloadDataAsync | {url}");
            }
            return null;
            
        }

        public class ApplicationDbContext : DbContext
        {
            public DbSet<Game> Games { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // Kết nối đến SQL Server
                optionsBuilder.UseSqlServer("Server=localhost; Database=GameXuaVNDb2; User Id=sa;password=abc12345-;Trusted_Connection=True; TrustServerCertificate=True;");
            }
        }


        public class Game
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int TotalPlay { get; set; }
            public int TotalLike { get; set; }
            public int TotalDislike { get; set; }
            public byte[] Thumbnail { get; set; }

            // Dữ liệu ảnh (binary)
            public byte[] Data { get; set; }

            // Kiểu MIME của file ảnh (ví dụ: image/jpeg, image/png)
            public string ContentType { get; set; }

            public int CategoryId { get; set; }

            public string EmbedUrl { get; set; }

            [MaxLength(2)]

            public string Page { get; set; } // #,A,B,C,D

            public DateTime CreationTime { get; set; }
            public bool IsDeleted { get; set; }

        }
    }
}
