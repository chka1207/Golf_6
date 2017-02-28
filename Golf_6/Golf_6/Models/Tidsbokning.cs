using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Golf_6.Models
{
    public class Tidsbokning
    {
        public int BokningsID { get; set; }

        public int BokareID { get; set; }

        [Required()]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required()]
        [DataType(DataType.Time)]
        public DateTime Tid { get; set; }

        [Display(Name ="GolfID för medspelare")]
        public string Spelare2ID { get; set; }
        [Display(Name = "GolfID för medspelare")]
        public string Spelare3ID { get; set; }
        [Display(Name = "GolfID för medspelare")]
        public String Spelare4ID { get; set; }


         
    }
}