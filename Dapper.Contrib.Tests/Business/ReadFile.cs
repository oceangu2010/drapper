using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Dapper.Contrib.Tests.Entity;

namespace Dapper.Contrib.Tests.Business
{
    public class ReadFile
    {
        // 读操作
        public static TicketData ReadTicketData()
        {
            // 读取文件的源路径及其读取流
            string strReadFilePath = @"ReceivedFile.txt";
            StreamReader srReadFile = new StreamReader(strReadFilePath);
            City city = null;
            List<City> cityList = null;
            SendProvince send = null;
            ArriveProvince arrive = null;
            List<SendProvince> sendList = new List<SendProvince>();
            List<ArriveProvince> arriveList = new List<ArriveProvince>();
            bool isSendProvince = true;//arrive then false
            string lineContent = string.Empty,content = string.Empty;
            string provinceName = string.Empty,cityName = string.Empty;
            int i = 0;
            string oldProvinceName = string.Empty;

            // 读取流直至文件末尾结束
            while (!srReadFile.EndOfStream)
            {
                lineContent = srReadFile.ReadLine(); //读取每行数据
                lineContent = lineContent.Trim();
                if (string.IsNullOrEmpty(lineContent))
                    continue;
                if (lineContent.IndexOf('=') <= -1)
                    continue;

                //去除 ;
                lineContent = lineContent.Substring(0, lineContent.Length - 1);

                //不存在，则new一个对象出来
                if ( i < 383 )
                {
                    isSendProvince = true;
                    if (send == null)
                        send = new SendProvince();
                }
                else if( i >= 383 )
                {
                    isSendProvince = false;

                    if (arrive == null)
                        arrive = new ArriveProvince();

                    if (send != null)
                    {
                        oldProvinceName = string.Empty;
                        send.CityData = cityList;
                        sendList.Add(send);
                        cityList = new List<City>();
                        send = null;
                    }
                }

                content = lineContent.Split('=')[1];
                //取省|城市名城
                if (string.IsNullOrEmpty(content) || content.IndexOf('|') <= -1)
                    continue;
                city = new City();
                content = content.Replace("\"", "");
                provinceName = content.Split('|')[0];
                cityName = content.Split('|')[1];
                city.CityName = cityName;

                if (i == 0)
                    cityList = new List<City>();

                if (isSendProvince)
                {
                    if (IsSendExit(sendList, provinceName) && (
                        string.IsNullOrEmpty(oldProvinceName) ||
                        oldProvinceName.ToLower().Equals(provinceName.ToLower())))
                    {
                        send.ProvinceName = provinceName;
                        cityList.Add(city);
                    }
                    else
                    {
                        if (i > 0)
                        {
                            send.CityData = cityList;
                            sendList.Add(send);
                        }
                        send = new SendProvince();
                        cityList = new List<City>();
                        send.ProvinceName = provinceName;
                        cityList.Add(city);
                    }
                }
                else
                {
                    if (IsArriveExit(arriveList, provinceName) && (
                        string.IsNullOrEmpty(oldProvinceName) ||
                        oldProvinceName.ToLower().Equals(provinceName.ToLower())))
                    {
                        cityList.Add(city);
                        arrive.ProvinceName = provinceName;
                    }
                    else
                    {
                        if (i > 0)
                        {
                            arrive.CityData = cityList;
                            arriveList.Add(arrive);
                        }
                        arrive = new ArriveProvince();
                        cityList = new List<City>();
                        arrive.ProvinceName = provinceName;
                        cityList.Add(city);
                    }
                }

                //最后
                if (!isSendProvince && i == 765)
                {
                    arrive.CityData = cityList;
                    arriveList.Add(arrive);
                }

                oldProvinceName = provinceName;

                i++;
            }
            // 关闭读取流文件
            srReadFile.Close();
            TicketData ticketData = new TicketData();
            ticketData.SendData = sendList;
            ticketData.ArriveData = arriveList;

            return ticketData;
        }

        private static bool IsSendExit(List<SendProvince> tList,string provicenName)
        {
            bool result = true;
            if (tList == null || tList.Count == 0)
                return true;
            foreach (var item in tList)
            {
                if (item == null)
                    continue;
                if (string.IsNullOrEmpty(item.ProvinceName))
                    continue;
                if (item.ProvinceName.ToLower().Equals(provicenName.ToLower()))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private static bool IsArriveExit(List<ArriveProvince> tList, string provicenName)
        {
            bool result = true ;
            if (tList == null || tList.Count ==0)
                return true;
            foreach (var item in tList)
            {
                if (item == null)
                    continue;
                if (string.IsNullOrEmpty(item.ProvinceName))
                    continue;
                if (item.ProvinceName.ToLower().Equals(provicenName.ToLower()))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
