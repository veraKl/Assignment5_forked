using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_EF_Start.Models
{
    public class Vehicle
    {
        [Key]
        [JsonProperty("vin_1_10")]
        public string VinNumber { get; set; }

        public string County { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public int ModelYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string EvType { get; set; }
        public string CafvType { get; set; }
        public int ElectricRange { get; set; }
        public decimal BaseMsrp { get; set; }
        public int LegislativeDistrict { get; set; }
        public string DolVehicleId { get; set; }

        [ForeignKey(nameof(StateNavigation))]
        public int StateId { get; set; }

        [ForeignKey(nameof(CountyNavigation))]
        public int CountyId { get; set; }

        [ForeignKey(nameof(MakeNavigation))]
        public int MakeId { get; set; }

        [ForeignKey(nameof(ModelNavigation))]
        public int ModelId { get; set; }


        // Navigation properties. 
        // helps with entity relationships
        public State StateNavigation { get; set; }
        public County CountyNavigation { get; set; }
        public Make MakeNavigation { get; set; }
        public Model ModelNavigation { get; set; }
        public Range RangeNavigation { get; set; }
    }
 

    public class State
    {
        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }

        // Navigation property for one-to-many relationship with County
        public ICollection<County> Counties { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }

    public class County
    {
        [Key]
        public int CountyId { get; set; }
        public string CountyName { get; set; }

        // Foreign key for the one-to-many relationship with State
        public int StateId { get; set; }
        public State State { get; set; }

        // Navigation properties for one-to-many relationships
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<Make> Makes { get; set; }
        public ICollection<Model> Models { get; set; }
    }

    public class Make
    {
        [Key]
        public int MakeId { get; set; }
        public string MakeName { get; set; }

        // Navigation property for one-to-many relationship with Model
        public ICollection<Model> Models { get; set; }

        // Navigation property for one-to-many relationship with Vehicle
        public ICollection<Vehicle> Vehicles { get; set; }
    }

    public class Model
    {
        [Key]
        public int ModelId { get; set; }
        public string ModelName { get; set; }

        // Foreign key for the one-to-many relationship with Make
        public int MakeId { get; set; }
        public Make Make { get; set; }

        // Navigation property for one-to-many relationship with Vehicle
        public ICollection<Vehicle> Vehicles { get; set; }
    }

    public class Range
    {
        [Key]
        public int RangeId { get; set; }
        public int ElectricRange { get; set; }

        // Foreign key for the one-to-one relationship with Vehicle
        [ForeignKey(nameof(Vehicle))]
        public string VinNumber { get; set; }
        public Vehicle Vehicle { get; set; }
    }

    // model for chart
    public class ChartModel
    {
        public string ChartType { get; set; }
        public string Labels { get; set; }

        public string Colors { get; set; }
        public string Data { get; set; }
        public string Title { get; set; }
    }

    // model for regional aggregation
    public class RegionViewModel
    {
        public string RegionName { get; set; }
        public int TotalCars { get; set; }
    }
}