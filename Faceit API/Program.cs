using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;

namespace Faceit_API
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            //await ProcessRepositories(dosent work);
            //await ProcessRepositories();
            await Webcrawler();
            Console.WriteLine("just for breakpoint");
        }
        
        private static async Task ProcessRepositories()
        {
            const string url = "https://open.faceit.com/data/v4/" +
                               "players?nickname=DKEagle" +
                               "&game=CS%3AGO" +
                               "&game_player_id=76561197960287930";

            var httpReqest = (HttpWebRequest) WebRequest.Create(url);
            httpReqest.Accept = "application/json";
            httpReqest.Headers["Authorization"] = "Bearer acecf83b-8e05-4a7f-9559-b8e5d20f5cda";

            var httpResponse = (HttpWebResponse) httpReqest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = await streamReader.ReadToEndAsync();
                Console.WriteLine(result);
                
            }
            Console.WriteLine(" ");
            Console.WriteLine(httpResponse.StatusCode);
        }


        private static async Task Webcrawler()
            {
                const string url = "https://www.ft.dk/";
                var httpclient = new HttpClient();
                var html = await httpclient.GetStringAsync(url);
            
                //HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get,url);
                //requestMessage.Headers.Add("User-Agent", "User-Agent-Here");
                //await httpclient.SendAsync(requestMessage);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                var div = htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", " ")
                        .Equals("read-more")).ToList();
                // ReSharper disable once CollectionNeverQueried.Local
                var infos = new List<Info>();

                foreach (var htmlnode in div)
                {
                    var info = new Info
                    {
                        InnerText = htmlnode?.Descendants("iquick-lookup-d__header")?.FirstOrDefault()?.InnerText
                    
                    };
                    infos.Add(info);
                    if (htmlnode != null) Console.WriteLine(htmlnode.InnerHtml);
                }
            }
            [DebuggerDisplay("{" + nameof(InnerText) + "}")]
            private class  Info
        {
            public string InnerText { get; init; }
            
        }
    }
}