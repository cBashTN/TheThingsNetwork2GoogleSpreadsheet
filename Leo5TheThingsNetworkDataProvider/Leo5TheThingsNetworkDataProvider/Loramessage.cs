namespace Leo5TheThingsNetworkDataProvider
{
    public class Loramessage
    {
        public double frequency { get; set; }
        public string datarate { get; set; }
        public string codingrate { get; set; }
        public long gateway_timestamp { get; set; }
        public int channel { get; set; }
        public string server_time { get; set; }
        public int rssi { get; set; }
        public double lsnr { get; set; }
        public int rfchain { get; set; }
        public int crc { get; set; }
        public string modulation { get; set; }
        public string gateway_eui { get; set; }
        public int altitude { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}