using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace Golf_6.Models
{
    public class ScorekortModel
    {
        //Relaterat till tees.
        public int tee { get; set; }
        public string teeNamn { get; set; }
        public DataTable allaTees { get; set; }

        public int kvinnaSlope { get; set; }
        public double kvinnaCr { get; set; }

        public int manSlope { get; set; }
        public double manCr { get; set; }


        //Relaterat till scorekort.
        public int sumMeter { get; set; }
        public DataTable scoreKort { get; set; }
        
        public int parFörstaHalvan { get; set; }
        public int parAndraHalvan { get; set; }
        public int parTotal { get; set; }
        public int banansPar { get; set; }

        public int slag { get; set; }
        public int räknare { get; set; }
        public int kvarvarande { get; set; }

        //Den informaiton som ska fylla scorekort.
        public Medlem AktuellMedlem { get; set; } = new Medlem();
        public Tidsbokning AktuellTidsbokning { get; set; } = new Tidsbokning();
        public List<ScorekortModel> Spelare { get; set; } = new List<ScorekortModel>();
    }
}