using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leo5TheThingsNetworkDataProvider
{
	public class Gateway
	{
		public string gtw_id { get; set; }
		public long timestamp { get; set; }
		public string time { get; set; }
		public double channel { get; set; }
		public double rssi { get; set; }
		public double snr { get; set; }
		public double rf_chain { get; set; }
		public double latitude { get; set; }
		public double longtitude { get; set; }
		public double altitude { get; set; }
	}
}
