using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Leo5TheThingsNetworkDataProvider;

namespace Keller.Leo5SwisscomDataProvider
{
    public static class Leo5TheThingNetworkPayloadConverter
    {

        public static Leo5TheThingNetworkPayload Convert(string payload)
        {
            byte[] payLoadBytes = ConvertHexStringToByteArray(payload);

            var data = new Leo5TheThingNetworkPayload
            {
                DeviceId = AppConstants.DeviceId,
                SwVersion = AppConstants.DeviceSwVersion,
                BatteryVoltageInMilliVolt = ExtractFloat(payLoadBytes, 4),
                ConnectedDeviceType = payLoadBytes[9].ToString() //0006 = P1 + TOB1
            };

            var first = new Channel
            {
                Type = ChannelType.P1, //because ConnectedDeviceType
                Value = ExtractFloat(payLoadBytes, 10)
            };
            data.FirstChannel = first;

            var second = new Channel
            {
                Type = ChannelType.TOB1, //because ConnectedDeviceType
                Value = ExtractFloat(payLoadBytes, 14)
            };
            data.SecondChannel = second;

            return data;
        }


        public static Leo5TheThingNetworkPayload Convert(byte[] payLoadBytes)
        {
            var data = new Leo5TheThingNetworkPayload
            {
                DeviceId = AppConstants.DeviceId,
                SwVersion = AppConstants.DeviceSwVersion,
                BatteryVoltageInMilliVolt = ExtractFloat(payLoadBytes, 4),
                ConnectedDeviceType = payLoadBytes[9].ToString() //0006 = P1 + TOB1
            };

            var first = new Channel
            {
                Type = ChannelType.P1, //because ConnectedDeviceType
                Value = ExtractFloat(payLoadBytes, 10)
            };
            data.FirstChannel = first;

            var second = new Channel
            {
                Type = ChannelType.TOB1, //because ConnectedDeviceType
                Value = ExtractFloat(payLoadBytes, 14)
            };
            data.SecondChannel = second;

            return data;
        }

        private static float ExtractFloat(byte[] payLoadBytes, int pos)
        {
            var bytes = new byte[4];
            Array.Copy(payLoadBytes, pos, bytes, 0, bytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            var hexAsBytes = new byte[hexString.Length / 2];

            if (hexString.Length % 2 != 0)
            {
                File.AppendAllText(AppConstants.LogStorageFilePath, $"The binary key cannot have an odd number of digits: {hexString}\n");
                
                //throw new ArgumentException("The binary key cannot have an odd number of digits: {hexString}");
            }

            for (var index = 0; index < hexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                hexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return hexAsBytes;
        }

    }
}