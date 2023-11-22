using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC_EF_Start.Models
{
    public class Regions
    {
        public string region_name { get; set; }
        public List<Counties> counties { get; set; }
    }

    public class Counties
    {
        public string county_name { get; set; }
        public Regions regions { get; set; }
        public Vehicles vehicles { get; set; }
    }

    public class Vehicles
    {
        public string vin { get; set; }
        public string make_name { get; set; }
        public string model_name { get; set; }
        public int range { get; set; }
        public Counties counties { get; set; }
        public Makes makes { get; set; }
        public Models models { get; set; }
    }

    public class Makes
    {
        public string make_name { get; set; }
        public string model_name { get; set; }
        public List<Vehicles> vehicles { get; set; }
        public List<Models> models { get; set; }
    }

    public class Models
    {
        public string model_name { get; set; }
        public Makes makes { get; set; }
        public Vehicles vehicles { get; set; }
    }
}