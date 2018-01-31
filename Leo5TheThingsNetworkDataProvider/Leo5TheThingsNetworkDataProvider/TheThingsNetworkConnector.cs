using System;
using System.IO;
using System.Text;
using System.Threading;
using Keller.Leo5SwisscomDataProvider;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Leo5TheThingsNetworkDataProvider
{
    public class TheThingsNetworkConnector
    {
        private bool _isRebootNeeded;
        public void ConnectAndListen()
        {

            //broker: staging.thethingsnetwork.org
            //port:     1883

            const string username = "appId"; //App Id
            const string password = "gGSymojL8Ch0ynwtBuUdYxQOdU1cG9HNCaQ/ltigFMk="; //Access Keys (not working version for github)
            const string topic = "appId/devices/#";
            string clientId = Guid.NewGuid().ToString();


            _isRebootNeeded = false;
            Thread.Sleep(5000);

            try
            {
                var client = new MqttClient("eu.thethings.network");    //Host: <Region>.thethings.network, where <Region> is last part of the handler you registered your application to, e.g. eu.
				client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

                var subscriptionId = client.Subscribe(new string[] { topic },new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                client.ConnectionClosed += Client_ConnectionClosed;
                client.MqttMsgSubscribed += Client_MqttMsgSubscribed;

                var connectionId = client.Connect(clientId, username, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Exception :" + ex.Message);
                File.AppendAllText(AppConstants.LogStorageFilePath, $"{DateTime.Now} - Error: {ex.Message}\n");

                _isRebootNeeded = true;
            }

            while (!_isRebootNeeded)
            {
                Thread.Sleep(100);
            }

        }


        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var now = DateTime.Now;

            //Deserialize the message into json
            var jsonText = Encoding.ASCII.GetString(e.Message);
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<LoraObject>(jsonText);

            //Decode from Base64
            byte[] decodedPayload = Convert.FromBase64String(message.payload_raw);

            //Log
            LogDecodedPayload(now, decodedPayload, message);

            //Decode payload
            Leo5TheThingNetworkPayload convertedPayload = Leo5TheThingNetworkPayloadConverter.Convert(decodedPayload);

            //Upload it to google sheet
            GoogleSheetWriter.Write(now, message, convertedPayload);
        }

        private static void LogDecodedPayload(DateTime now, byte[] decodedPayload, LoraObject message)
        {
            var s = new string(Encoding.UTF8.GetString(decodedPayload).ToCharArray());
            string text = $"{now} : dev_eui: {message.dev_id} with payload: {Encoding.Default.GetString(decodedPayload)} ({s}) from {message.metadata.gateways.Count} gateways. Nr. {message.counter}\n";
            Console.WriteLine(text);
            File.AppendAllText(AppConstants.DataStorageFilePath, text);
        }


        //private string Base64DecodeAndStringify(string base64EncodedData)
        //{
        //    var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var bytes in base64EncodedBytes)
        //    {
        //        string x = bytes.ToString("X").Replace((char) 0xA, '0');
        //        //x must be two chars
        //        sb.Append(x);
        //    }
        //    return sb.ToString();
        //}



        private void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Client_MqttMsgSubscribed: " + e.ToString());
            File.AppendAllText(AppConstants.LogStorageFilePath, $"{DateTime.Now} - Client_MqttMsgSubscribed: {e}\n");
        }

        private void Client_ConnectionClosed(object sender, EventArgs e)
        {
            Console.WriteLine("Client_ConnectionClosed: " + e.ToString());
            File.AppendAllText(AppConstants.LogStorageFilePath, $"{DateTime.Now} - Client_ConnectionClosed: {e}\n");
            _isRebootNeeded = true;
        }

    }
}