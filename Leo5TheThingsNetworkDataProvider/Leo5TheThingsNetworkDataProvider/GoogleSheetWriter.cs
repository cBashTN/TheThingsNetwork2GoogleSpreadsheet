using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Keller.Leo5SwisscomDataProvider;
using Newtonsoft.Json;

namespace Leo5TheThingsNetworkDataProvider
{
    public static class GoogleSheetWriter
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json (CredPath)
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private const string ApplicationName = "Google Sheets API .NET Quickstart";

        internal static void Write(DateTime now, LoraObject msg, Leo5TheThingNetworkPayload payloadInfo)
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) //You get this credential file from Google. It's not part of public github solution.
            {

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(AppConstants.CredPath, true)).Result;

                //Console.WriteLine("Credential file saved to: " + CredPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(AppConstants.SpreadsheetId, AppConstants.Range);

            ValueRange sheetData = request.Execute();
            IList<IList<object>> values = sheetData.Values;


            if (values == null || values.Count == 0)
            {
                File.AppendAllText(AppConstants.LogStorageFilePath, "GoogleSheetWriter: No data found.\n");
                //todo add return after first run          
                //return;
            }


          //  var lastMessage = sheetData.Values[sheetData.Values.Count - 1];

            //if (((string)lastMessage[1] == msg.Time.ToString()  ) &&
            //    ((string)lastMessage[3] == msg.FCntUp.ToString()) && 
            //    ((string)lastMessage[4] == msg.FCntDn.ToString()))
            //{
            //    return;
            //}


            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-CH");

            Loramessage bestSignalGateway;
            string bestSignalGateWayEUI;
            
            if (msg.metadata.Count > 1)
            {
                bestSignalGateway = msg.metadata.OrderByDescending(item => item.rssi).First();
                bestSignalGateWayEUI = bestSignalGateway.gateway_eui;

            }
            else
            {
                bestSignalGateway = msg.metadata[0];
                bestSignalGateWayEUI = bestSignalGateway.gateway_eui;
            }

            string gatewayDetails = JsonConvert.SerializeObject(msg);

            var newRow = new List<object>
            {
                msg.metadata[0].server_time,
                now.ToString("dd.MM.yyyy HH:mm:ss"),

                payloadInfo.FirstChannel.Type.ToString(),
                payloadInfo.FirstChannel.Value.ToString(CultureInfo.CurrentCulture),
                payloadInfo.SecondChannel.Type.ToString(),
                payloadInfo.SecondChannel.Value.ToString(CultureInfo.CurrentCulture),

                payloadInfo.BatteryVoltageInMilliVolt.ToString(CultureInfo.CurrentCulture),

                msg.counter.ToString(),
                msg.metadata.Count.ToString(),
                bestSignalGateWayEUI,
                
                bestSignalGateway.lsnr,
                bestSignalGateway.rssi,
                bestSignalGateway.datarate,
                bestSignalGateway.latitude,
                bestSignalGateway.longitude,


                gatewayDetails
               

            };

            if (sheetData.Values.Count > AppConstants.MaxFields)
            {
                sheetData.Values.RemoveAt(1);
            }

            sheetData.Values.Add(newRow);

            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest =
                service.Spreadsheets.Values.Update(sheetData, AppConstants.SpreadsheetId, sheetData.Range);
            updateRequest.ValueInputOption =
                SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            UpdateValuesResponse updateResponse = updateRequest.Execute();

            File.AppendAllText(AppConstants.LogStorageFilePath, $"{updateResponse.UpdatedCells}, {updateResponse.UpdatedColumns}, {updateResponse.UpdatedRows}\n");
        }
    }
}