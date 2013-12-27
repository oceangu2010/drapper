using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace DapperWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetPost();
        }

        private void Get()
        {
            string postString = "fn_busline=2_1&fd_date1=2014-01-10&fd_date1_dp=1&fd_date1_year_start=2013&fd_date1_year_end=2014&fd_date1_da1=1388077200&fd_date1_da2=1419613200&fd_date1_sna=1&fd_date1_aut=&fd_date1_frm=&fd_date1_tar=&fd_date1_inp=&fd_date1_fmt=l+d+F+Y&fd_date1_dis=&fd_date1_pr1=&fd_date1_pr2=&fd_date1_prv=&fd_date1_pth=calendar%2F&fd_date1_spd=%5B%5B%5D%2C%5B%5D%2C%5B%5D%5D&fd_date1_spt=0&fd_date1_och=&fd_date1_str=1&fd_date1_rtl=0&fd_date1_wks=&fd_date1_int=1&fd_date1_hid=0&fd_date1_hdt=3000&fd_date1_hl=th_TH&fd_date1_dig=0&fd_date1_ttd=%5B%5B%5D%2C%5B%5D%2C%5B%5D%5D&fd_date1_ttt=%255B%255B%255D%252C%255B%255D%252C%255B%255D%255D&btn_filter=%26%2323637%3B%26%2331034%3B%3E%3E";//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
            string url = "http://www.nakhonchaiair.com/ncabooking/frm_ncabooking.php";//地址  
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
            string srcString = Encoding.UTF8.GetString(responseData);//解
        }

        private string GetPost()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.nakhonchaiair.com/ncabooking/frm_ncabooking.php");  
            request.CookieContainer = new CookieContainer();  
            CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可  
            //以下是发送的http头，随便加，其中referer挺重要的，有些网站会根据这个来反盗链  
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
            string postDataStr = "fn_busline=2_1&fd_date1=2014-01-10&fd_date1_dp=1&fd_date1_year_start=2013&fd_date1_year_end=2014&fd_date1_da1=1388077200&fd_date1_da2=1419613200&fd_date1_sna=1&fd_date1_aut=&fd_date1_frm=&fd_date1_tar=&fd_date1_inp=&fd_date1_fmt=l+d+F+Y&fd_date1_dis=&fd_date1_pr1=&fd_date1_pr2=&fd_date1_prv=&fd_date1_pth=calendar%2F&fd_date1_spd=%5B%5B%5D%2C%5B%5D%2C%5B%5D%5D&fd_date1_spt=0&fd_date1_och=&fd_date1_str=1&fd_date1_rtl=0&fd_date1_wks=&fd_date1_int=1&fd_date1_hid=0&fd_date1_hdt=3000&fd_date1_hl=th_TH&fd_date1_dig=0&fd_date1_ttd=%5B%5B%5D%2C%5B%5D%2C%5B%5D%5D&fd_date1_ttt=%255B%255B%255D%252C%255B%255D%252C%255B%255D%255D&btn_filter=%26%2323637%3B%26%2331034%3B%3E%3E";//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
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
