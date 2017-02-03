namespace Keller.Leo5SwisscomDataProvider
{
    public class Leo5TheThingNetworkPayload
    {
        public string DeviceId; //e.g. "30.5"=Leo5
        public string SwVersion; //e.g. "16.12"=Leo5 with Lora

        public float BatteryVoltageInMilliVolt;

        public string ConnectedDeviceType; //e.g. "30.5"=Leo5

        public int ActiveChannels; //06 dec --> 0000 1010 bin -> P1 active, ToB1 active 

        public Channel FirstChannel;
        public Channel SecondChannel;
        //...
    }

    public class Channel
    {
        public ChannelType Type;
        public float Value;
    }

    public enum ChannelType
    {
        P1,
        P2,
        TOB1,
        TOB2,
        //...
    }
}