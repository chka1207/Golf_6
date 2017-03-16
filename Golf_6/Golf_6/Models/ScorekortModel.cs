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
        public int tee { get; set; } = 0;
        public string teeNamn { get; set; } = "";
        public DataTable allaTees { get; set; }

        public int kvinnaSlope { get; set; } = 0;
        public double kvinnaCr { get; set; } = 0;

        public int manSlope { get; set; } = 0;
        public double manCr { get; set; } = 0;


        //Relaterat till scorekort.
        public int sumMeter { get; set; } = 0;
        public DataTable scoreKort { get; set; }

        public int parFörstaHalvan { get; set; } = 0;
        public int parAndraHalvan { get; set; } = 0;
        public int parTotal { get; set; } = 0;
        public int banansPar { get; set; } = 0;

        public int slag { get; set; } = 0;
        public int räknare { get; set; } = 0;
        public int kvarvarande { get; set; } = 0;

        //Den informaiton som ska fylla scorekort.
        public Medlem AktuellMedlem { get; set; } = new Medlem();
        public Tidsbokning AktuellTidsbokning { get; set; } = new Tidsbokning();
        public List<ScorekortModel> Spelare { get; set; } = new List<ScorekortModel>();
    }
}