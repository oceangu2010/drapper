using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Web;
using System.Net;
using System.IO.Compression;
using Dapper.Contrib.Tests.Business;

namespace Dapper.Contrib.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup();
            //RunTests();

            /*
           string result = PostRequest.GetAjaxArriveCity();
           Console.WriteLine(result);*/

           string result2 = PostRequest.GetPostBusticket();
           Console.WriteLine(result2);

            /*
           Dapper.Contrib.Tests.Entity.TicketData ticketData = ReadFile.ReadTicketData();
           int sendCityCount = 0, arriveCityCount = 0;
           if (ticketData != null)
           {
               ticketData.SendData.ForEach(item => sendCityCount += item.CityData.Count);
               ticketData.ArriveData.ForEach(item => arriveCityCount += item.CityData.Count);
           }

           Console.WriteLine("send city number:" + sendCityCount.ToString() + "(原:383)");
           Console.WriteLine("arrive city number:" + arriveCityCount.ToString() + "(原:383)");
            */

           Console.ReadKey();
        }

        private static string GetPost()
        {
            string retString = string.Empty;
            try
            {
                WebClient wc = new WebClient();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.nakhonchaiair.com/ncabooking/frm_ncabooking.php");
               // CookieContainer cookie = new CookieContainer();//request.CookieContainer;//wc.ResponseHeaders[HttpResponseHeader.SetCookie];//如果用不到Cookie，删去即可  
               // request.CookieContainer = cookie;//new CookieContainer();  

                //以下是发送的http头，随便加，其中referer挺重要的，有些网站会根据这个来反盗链  
                //string postDataStr = "fn_busline=2_1&fd_date1=2014-01-10&fd_date1_dp=1&fd_date1_year_start=2013&fd_date1_year_end=2014&fd_date1_da1=1388077200&fd_date1_da2=1419613200&fd_date1_sna=1&fd_date1_aut=&fd_date1_frm=&fd_date1_tar=&fd_date1_inp=&fd_date1_fmt=l+d+F+Y&fd_date1_dis=&fd_date1_pr1=&fd_date1_pr2=&fd_date1_prv=&fd_date1_pth=calendar%2F&fd_date1_spd=%5B%5B%5D%2C%5B%5D%2C%5B%5D%5D&fd_date1_spt=0&fd_date1_och=&fd_date1_str=1&fd_date1_rtl=0&fd_date1_wks=&fd_date1_int=1&fd_date1_hid=0&fd_date1_hdt=3000&fd_date1_hl=th_TH&fd_date1_dig=0&fd_date1_ttd=%5B%5B%5D%2C%5B%5D%2C%5B%5D%5D&fd_date1_ttt=%255B%255B%255D%252C%255B%255D%252C%255B%255D%255D&btn_filter=%26%2323637%3B%26%2331034%3B%3E%3E";//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
                string postDataStr = "fn_busline=3_1&fd_date1=2014-01-10&pd_date=2014-01-10&pn_src=3&pn_des=1&pn_busline=2&pn_buslinetype=2&pn_bustype=1&pn_srctime=2000&pn_leavetime=2000&fn_leavetime=15";
                request.Host = "www.nakhonchaiair.com";
                //<SPAN class=key>request.Headers.Add(HttpRequestHeader.Cookie, "ASPSESSIONIDSCATBTAD=KNNDKCNBONBOOBIHHHHAOKDM;");</SPAN> 
                request.Headers.Add(HttpRequestHeader.Cookie, "PHPSESSID=nmmb0min43msf4sehktpl8o2j0");//PHPSESSID=234fe4859v8rtmivsv5mnuk4d6
                request.Referer = "http://www.nakhonchaiair.com/ncabooking/frm_ncabooking.php";
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
                retString = streamReader.ReadToEnd();

                streamReader.Close();
                responseStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return retString;
        }

        private static void Setup()
        {
            var projLoc = Assembly.GetAssembly(typeof(Program)).Location;
            var projFolder = Path.GetDirectoryName(projLoc);

            if (File.Exists(projFolder + "\\Test.sdf"))
                File.Delete(projFolder + "\\Test.sdf");
            var connectionString = "Data Source = " + projFolder + "\\Test.sdf;";
            var engine = new SqlCeEngine(connectionString);
            engine.CreateDatabase();
            using (var connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                connection.Execute(@" create table Users (Id int IDENTITY(1,1) not null, Name nvarchar(100) not null, Age int not null) ");
                connection.Execute(@" create table Automobiles (Id int IDENTITY(1,1) not null, Name nvarchar(100) not null) ");
                connection.Execute(@" create table Results (Id int IDENTITY(1,1) not null, Name nvarchar(100) not null, [Order] int not null) ");
            }
            Console.WriteLine("Created database");
        }

        private static void RunTests()
        {
            var tester = new Tests();
            foreach (var method in typeof(Tests).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Console.Write("Running " + method.Name);
                method.Invoke(tester, null);
                Console.WriteLine(" - OK!");
            }
            Console.ReadKey();
        }




    }
}
