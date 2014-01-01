using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;

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
    }
}
