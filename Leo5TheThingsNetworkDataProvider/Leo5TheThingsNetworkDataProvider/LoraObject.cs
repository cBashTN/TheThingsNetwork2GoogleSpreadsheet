using System.Collections.Generic;

namespace Leo5TheThingsNetworkDataProvider
{
    public class LoraObject
    {
		public string app_id { get; set; }
		public string dev_id { get; set; }
		public string hardware_serial { get; set; }
		public int port { get; set; }
		public int counter { get; set; }
		public string confurmed { get; set; }
		public string payload_raw { get; set; }

		public Loramessage metadata { get; set; }
		//can be payload_fields yet
	}
}