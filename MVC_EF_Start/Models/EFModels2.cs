using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_EF_Start.Models
{
    public class EFModels2
    {
        public class DRegion
        {
            public int RegionID { get; set; }
            public string RegionName { get; set; }

            public virtual ICollection<County> Counties { get; set; }
        }

        public class DCounty
        {
            public int CountyID { get; set; }
            public string CountyName { get; set; }

            public int RegionID { get; set; } // Foreign key
            public virtual DRegion Region { get; set; }

            public virtual ICollection<Vehicle> Vehicles { get; set; }
            public virtual ICollection<Make> Makes { get; set; }
            public virtual ICollection<Model> Models { get; set; }
        }

        public class DMake
        {
            public int MakeID { get; set; }
            public string MakeName { get; set; }

            public int CountyID { get; set; } // Foreign key
            public virtual County County { get; set; }

            public virtual ICollection<Model> Models { get; set; }
        }

        public class DModel
        {
            public int ModelID { get; set; }
            public string ModelName { get; set; }

            public int CountyID { get; set; } // Foreign key
            public virtual County County { get; set; }

            public virtual ICollection<Vehicle> Vehicles { get; set; }
        }

        public class DVehicle
        {
            [Key]
            public string VIN { get; set; }
            public string Make { get; set; }
            public int MakeID { get; set; }
            public string Model { get; set; }
            public int ModelID { get; set; }
            public int Range { get; set; }

            public int CountyID { get; set; } // Foreign key
            public virtual County County { get; set; }
        }


        public class ExcelDataViewModel
        {
            public string VIN { get; set; }
            public string County { get; set; }
            public string State { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public int ElectricRange { get; set; }
        }

    }
}
