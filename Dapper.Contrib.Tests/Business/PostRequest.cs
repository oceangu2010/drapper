using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Dapper.Contrib.Tests.Business
{
    public class PostRequest
    {

        public static string GetAjaxArriveCity()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://booking.busticket.in.th/ajax_getarrivecity.php ");//
            request.CookieContainer = new CookieContainer();
            CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可  
            //以下是发送的http头，随便加，其中referer挺重要的，有些网站会根据这个来反盗链http://booking.busticket.in.th/availabletrip.php  
            string postDataStr = "departcity=กรุงเทพมหานคร&departterminal=สถานีขนส่งผู้โดยสารกรุงเทพฯ (หมอชิต 2)&defaultcity=";

            request.Referer = "http://booking.busticket.in.th/index.php";
            request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8,en;q=0.6,zh-TW;q=0.4";
            request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
            request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            request.KeepAlive = true;
            //上面的http头看情况而定，但是下面俩必须加  
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            Encoding encoding = Encoding.UTF8;//根据网站的编码自定义  
            //string postDataStr = "";
            byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();

            return retString;
        }

        public static string GetPostBusticket()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://booking.busticket.in.th/availabletrip.php");//
            request.CookieContainer = new CookieContainer();
            CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可  
            //以下是发送的http头，随便加，其中referer挺重要的，有些网站会根据这个来反盗链
            //这里传值使用了urlencode加密方式
            string postDataStr = "triptype=oneway&departcity=%E0%B8%81%E0%B8%A3%E0%B8%B8%E0%B8%87%E0%B9%80%E0%B8%97%E0%B8%9E%E0%B8%A1%E0%B8%AB%E0%B8%B2%E0%B8%99%E0%B8%84%E0%B8%A3&departterminal=%E0%B8%AA%E0%B8%96%E0%B8%B2%E0%B8%99%E0%B8%B5%E0%B8%82%E0%B8%99%E0%B8%AA%E0%B9%88%E0%B8%87%E0%B8%9C%E0%B8%B9%E0%B9%89%E0%B9%82%E0%B8%94%E0%B8%A2%E0%B8%AA%E0%B8%B2%E0%B8%A3%E0%B8%81%E0%B8%A3%E0%B8%B8%E0%B8%87%E0%B9%80%E0%B8%97%E0%B8%9E%E0%B8%AF+%28%E0%B8%AB%E0%B8%A1%E0%B8%AD%E0%B8%8A%E0%B8%B4%E0%B8%95+2%29&arrivecity=%E0%B8%81%E0%B8%B3%E0%B9%81%E0%B8%9E%E0%B8%87%E0%B9%80%E0%B8%9E%E0%B8%8A%E0%B8%A3&arriveterminal=%E0%B8%AA%E0%B8%96%E0%B8%B2%E0%B8%99%E0%B8%B5%E0%B8%82%E0%B8%99%E0%B8%AA%E0%B9%88%E0%B8%87%E0%B8%9C%E0%B8%B9%E0%B9%89%E0%B9%82%E0%B8%94%E0%B8%A2%E0%B8%AA%E0%B8%B2%E0%B8%A3+%E0%B8%88.%E0%B8%81%E0%B8%B3%E0%B9%81%E0%B8%9E%E0%B8%87%E0%B9%80%E0%B8%9E%E0%B8%8A%E0%B8%A3&departdate=11&departmonth=01%2F2014&Submit=%E0%B8%84%E0%B9%89%E0%B8%99%E0%B8%AB%E0%B8%B2%E0%B9%80%E0%B8%97%E0%B8%B5%E0%B9%88%E0%B8%A2%E0%B8%A7%E0%B8%A3%E0%B8%96%21";
            //&Submit=%E5%AF%BB%E6%89%BE%E4%B8%80%E4%B8%AA%E8%BD%A6%EF%BC%81";//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
            // string postDataStr = "triptype=oneway&departcity=%E0%B8%95%E0%B8%B2%E0%B8%81&departterminal=%E0%B8%AA%E0%B8%96%E0%B8%B2%E0%B8%99%E0%B8%B5%E0%B8%82%E0%B8%99%E0%B8%AA%E0%B9%88%E0%B8%87%E0%B8%9C%E0%B8%B9%E0%B9%89%E0%B9%82%E0%B8%94%E0%B8%A2%E0%B8%AA%E0%B8%B2%E0%B8%A3+%E0%B8%88.%E0%B8%95%E0%B8%B2%E0%B8%81&arrivecity=%E0%B8%81%E0%B8%A3%E0%B8%B8%E0%B8%87%E0%B9%80%E0%B8%97%E0%B8%9E%E0%B8%A1%E0%B8%AB%E0%B8%B2%E0%B8%99%E0%B8%84%E0%B8%A3&arriveterminal=%E0%B8%AA%E0%B8%96%E0%B8%B2%E0%B8%99%E0%B8%B5%E0%B8%82%E0%B8%99%E0%B8%AA%E0%B9%88%E0%B8%87%E0%B8%9C%E0%B8%B9%E0%B9%89%E0%B9%82%E0%B8%94%E0%B8%A2%E0%B8%AA%E0%B8%B2%E0%B8%A3%E0%B8%81%E0%B8%A3%E0%B8%B8%E0%B8%87%E0%B9%80%E0%B8%97%E0%B8%9E%E0%B8%AF+%28%E0%B8%AB%E0%B8%A1%E0%B8%AD%E0%B8%8A%E0%B8%B4%E0%B8%95+2%29&departdate=2&departmonth=01%2F2014&Submit=%E5%AF%BB%E6%89%BE%E4%B8%80%E4%B8%AA%E8%BD%A6%EF%BC%81";

            request.Referer = "http://booking.busticket.in.th/index.php";
            request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8,en;q=0.6,zh-TW;q=0.4";
            request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
            request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            request.KeepAlive = true;
            //上面的http头看情况而定，但是下面俩必须加  
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            
            Encoding encoding = Encoding.UTF8;//根据网站的编码自定义  
            //string postDataStr = "";
            byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();

            //GroupCollection reg = Regex.Match(retString, @"<table.*(?=title_darklight_13)(.|/n)*?</table>").Groups; //(class=\042title_darklight_13\042)
            string tableHtml = Regex.Match(retString, @"<table.*?(class=\""title_darklight_13\"")>[\s\S]*?<\/table>",RegexOptions.Multiline|RegexOptions.IgnoreCase).Value;

            MatchCollection trCollection = Regex.Matches(tableHtml, @"<tr[^<]*\>(?:\s*<td[^>]*>(.*?)</td>)*\s*</tr>",RegexOptions.Singleline|RegexOptions.IgnoreCase);

            if (trCollection != null && trCollection.Count > 0)
            {
                int m = 0;
                foreach (Match item in trCollection)
                {
                    //table header先剔除
                    //table foot 也要剔除
                    if (m == 0 || m == trCollection.Count - 1 ) 
                    {
                        m++;
                        continue;
                    }
                    foreach(Capture c in item.Groups[1].Captures)
                        Console.WriteLine(c.Value);
                    m++;
                }
            }

            return retString;
        }

        //可以用来判断是否有票
        public static  void RegTest(string htmls="")
        {

            string html = "<div align=\"center\"><input type=\"radio\" name=\"departtrip\" value=\"dHJhbnNwb3J0fDIzNjQyNDZ8fDY3OXwyMDE0LTAxLTExfDE2OjMwfOC5gOC4iuC4teC4ouC4h+C5g+C4q+C4oeC5iHzguIjguLjguJTguIjguK3guJQg4LitLuC4neC4suC4h3zguIHguKPguLjguIfguYDguJfguJ7guKHguKvguLLguJnguITguKN84LiB4Lij4Lih4LiB4Liy4Lij4LiC4LiZ4Liq4LmI4LiH4LiX4Liy4LiH4Lia4LiBfDI3NnwyNzZ8MTIyM3wxMjIzfOC4muC4o+C4tOC4qeC4seC4lyDguILguJnguKrguYjguIcg4LiI4Liz4LiB4Lix4LiUfHx84LiB4Lij4Li44LiH4LmA4LiX4Lie4LivIC0g4Lia4LmJ4Liy4LiZ4LiX4LmI4Liy4LiV4Lit4LiZfDE2OjMwfE4vQXx84LihLjTguIJ8WQ==\" onclick=\"departTripClicked('transport','บริษัท ขนส่ง จำกัด','ม.4ข',679,276,1223)\"></div>";
            string inputName = "departtrip";
            GetInput(html, inputName);
        }

        public static string GetInput(string FileString, string inputName)
        {
            string inputValue = "";
            MatchCollection matches = Regex.Matches(FileString, @"(?is)<input[^>]+type=""(?:radio)""[^>]+name=""(?:" + inputName + @")""[^>]+value=""(.+?)""[^>]*>");
            foreach (Match match in matches)
                inputValue += match.Groups[1].Value;

            return inputValue;
        }
    }
}
