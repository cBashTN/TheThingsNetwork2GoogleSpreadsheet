using System.Collections.Generic;

namespace Leo5TheThingsNetworkDataProvider
{
    public class Loramessage
    {
		public string time { get; set; }
		public double frequency { get; set; }
		public string modulation { get; set; }
		public string data_rate { get; set; }
		public long airtime { get; set; }
		public double bit_rate { get; set; }

		public string coding_rate { get; set; }
		public List<Gateway> gateways { get; set; }
		public int altitude { get; set; }
		public double longitude { get; set; }
		public double latitude { get; set; }
	}
}