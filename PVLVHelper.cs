using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace PacketViewerLogViewer.PVLVHelper
{

    public class YoutubeHelperVideoLink
    {
        public string VideoURL;
        public string QualityName;
    }

    static class YoutubeHelper
    {
        static public List<YoutubeHelperVideoLink> GetVideoURLs(string YoutubeURL)
        {
            List<YoutubeHelperVideoLink> res = new List<YoutubeHelperVideoLink>();
            var yt = new YoutubeUrlResolver();
            var links = yt.Extractor(YoutubeURL);
            foreach (var link in links)
            {
                YoutubeHelperVideoLink vl = new YoutubeHelperVideoLink();
                vl.VideoURL = link.ElementAt(0);
                vl.QualityName = link.ElementAt(1);
                res.Add(vl);
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

    static class Helper
    {
        // Source: https://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path
        /// <summary>
        /// Creates a relative path from one file
        /// or folder to another.
        /// </summary>
        /// <param name="fromDirectory">
        /// Contains the directory that defines the
        /// start of the relative path.
        /// </param>
        /// <param name="toPath">
        /// Contains the path that defines the
        /// endpoint of the relative path.
        /// </param>
        /// <returns>
        /// The relative path from the start
        /// directory to the end path.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string MakeRelative(string fromDirectory, string toPath)
        {
            if (fromDirectory == null)
                throw new ArgumentNullException("fromDirectory");

            if (toPath == null)
                throw new ArgumentNullException("toPath");

            bool isRooted = (Path.IsPathRooted(fromDirectory) && Path.IsPathRooted(toPath));

            if (isRooted)
            {
                bool isDifferentRoot = (string.Compare(Path.GetPathRoot(fromDirectory), Path.GetPathRoot(toPath), true) != 0);

                if (isDifferentRoot)
                    return toPath;
            }

            List<string> relativePath = new List<string>();
            string[] fromDirectories = fromDirectory.Split(Path.DirectorySeparatorChar);

            string[] toDirectories = toPath.Split(Path.DirectorySeparatorChar);

            int length = Math.Min(fromDirectories.Length, toDirectories.Length);

            int lastCommonRoot = -1;

            // find common root
            for (int x = 0; x < length; x++)
            {
                if (string.Compare(fromDirectories[x], toDirectories[x], true) != 0)
                    break;

                lastCommonRoot = x;
            }

            if (lastCommonRoot == -1)
                return toPath;

            // add relative folders in from path
            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
            {
                if (fromDirectories[x].Length > 0)
                    relativePath.Add("..");
            }

            // add to folders to path
            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
            {
                relativePath.Add(toDirectories[x]);
            }

            // create relative path
            string[] relativeParts = new string[relativePath.Count];
            relativePath.CopyTo(relativeParts, 0);

            string newPath = string.Join(Path.DirectorySeparatorChar.ToString(), relativeParts);

            return newPath;
        }

    }

}
