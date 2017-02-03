using System.Collections.Generic;

namespace Leo5TheThingsNetworkDataProvider
{
    public class LoraObject
    {
        public string payload { get; set; }
        public int port { get; set; }
        public int counter { get; set; }
        public string dev_eui { get; set; }
        public List<Loramessage> metadata { get; set; }
    }
}