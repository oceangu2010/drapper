using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper.Contrib.Tests.Entity
{
    public class PlaceNameEntity
    {
    }

    [Serializable]
    public class SendProvince
    {
        public string ProvinceName { get; set; }
        public List<City> CityData { get; set; }
    }

    [Serializable]
    public class ArriveProvince
    {
        public string ProvinceName { get; set; }
        public List<City> CityData { get; set; }
    }

    [Serializable]
    public class City
    {
        public string CityName{ get;set; }
    }

    [Serializable]
    public class TicketData
    {
        public List<SendProvince> SendData { get; set; }
        public List<ArriveProvince> ArriveData { get; set; }
    }
}
