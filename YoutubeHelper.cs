using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PacketViewerLogViewer.YoutubeVideoHelper
{
    static class YoutubeHelper
    {

        static public List<string> GetVideoURLs(string YoutubeURL)
        {
            List<string> res = new List<string>();
            var yt = new YoutubeUrlResolver();
            var links = yt.Extractor(YoutubeURL);
            foreach (var link in links)
            {
                res.Add(link.ElementAt(0));
                //Console.WriteLine(link.ElementAt(0) + "\n"); // url of the video file at a particular resolution
                //Console.WriteLine(link.ElementAt(1) + "\n\n"); //quality of the video file
            }
            return res;
        }
    }

    public class YoutubeUrlResolver
    {

        public List<List<string>> Extractor(string url)
        {

            var html_content = "";
            using (var client = new WebClient())

            {
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2227.1 Safari/537.36");
                html_content += client.DownloadString(url);
            }

            var Regex1 = new Regex(@"url=(.*?tags=\\u0026)", RegexOptions.Multiline);
            var matched = Regex1.Match(html_content);
            var download_infos = new List<List<string>>();
            foreach (var matched_group in matched.Groups)
            {
                var urls = Regex.Split(WebUtility.UrlDecode(matched_group.ToString().Replace("\\u0026", " &")), ",?url=");

                foreach (var vid_url in urls.Skip(1))
                {
                    var download_url = vid_url.Split(' ')[0].Split(',')[0].ToString();
                    Console.WriteLine(download_url);

                    // for quality info of the video
                    var Regex2 = new Regex("(quality=|quality_label=)(.*?)(,|&| |\")");
                    var QualityInfo = Regex2.Match(vid_url);
                    var quality = QualityInfo.Groups[2].ToString(); //quality_info
                    download_infos.Add((new List<string> { download_url, quality })); //contains url and resolution

                }
            }
            return download_infos;
        }

    }

}
