using System;
using System.Linq;
using System.Net;
using EverywhereServerClientSample.zenon;

namespace EverywhereServerClientSample
{
    class Program
    {
        private const string ClientId = "D28C0097-CF46-4852-9411-D315F0816295";
        private const string ProjectName = "SUPERVISOR760";
        private const string Username = "zenonusername";
        private const string Password = "46&!qMC#2luI";

        static void Main(string[] args)
        {
            // For ODATA usage, see:
            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v3/calling-an-odata-service-from-a-net-client

            // For documentation of Everywhere Server, see zenon Documentation:
            // Manual -> Mobile applications for zenon -> Everywhere Server by zenon
            // This sample uses demo project SUPERVISOR760, add user and password to access the project.

            try
            {
                // When using a temporary certificate, this line can be uncommented for testing only purposes

                // -- testing only -- testing only -- testing only -- testing only -- testing only 
                //ServicePointManager.ServerCertificateValidationCallback =
                //    (sender, certificate, chain, sslPolicyErrors) => true;
                // -- testing only -- testing only -- testing only -- testing only -- testing only 

                Uri uri = new Uri($"https://localhost:8050/zendata/{ProjectName}/");

                // Cerate DataServiceContext
                string unixTimestamp = ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString("X4");
                var zenonDataModel = new ZenonDataModel(uri)
                {
                    Credentials = new NetworkCredential
                    {
                        UserName = $"{unixTimestamp}|{ClientId}|{Username}",
                        Password = $"={Password}"
                    }
                };

                // Query variable and get value
                var variableQuery = zenonDataModel.Variables.Where(v => v.Name == "Pharma_Process_PrimaryReactor2.level");
                var variable = variableQuery.First();

                ZenonOnlineVariable zenonOnlineVariable = new ZenonOnlineVariable { Id = variable.Id };
                zenonDataModel.AddToOnlineVariables(zenonOnlineVariable);

                zenonDataModel.SaveChanges();

                // Output value
                Console.WriteLine($"Variable value is \"{zenonOnlineVariable.Value}\".");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
