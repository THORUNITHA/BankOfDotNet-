using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankOfDotNet.ConsoleClient
{
    class Program
    {

        //Code to Consume IdentityServer

        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();
        private static async Task MainAsync()
        {

            var discoRO = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (discoRO.IsError)
            {
                Console.WriteLine(discoRO.Error);
                return;
            }
            //Grab a bearer token using Resource Owner
            var tokenClientRO = new TokenClient(discoRO.TokenEndpoint, "ro.client", "secret");
            var tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync("Thorunitha", "password", "BankOfDotNetApi");
            if (tokenResponseRO.IsError)
            {
                Console.WriteLine(tokenResponseRO.Error);
                return;
            }
            Console.WriteLine(tokenResponseRO.Json);
            Console.WriteLine("\n\n");




            //Discover all the endpoints using metadata of identity server
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            //connect token and request token
            var tokenClient = new TokenClient(disco.TokenEndpoint,"client","secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("BankOfDotNetApi");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("/n/n");

            //consume our customer API
            var client = new HttpClient();
            //Set Client Authorization header
            client.SetBearerToken(tokenResponse.AccessToken);
            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                    new { Id = 10, FirstName = "sathiya", LastName = "Narayanan" }
                    ), Encoding.UTF8, "application/json");
            var createCustomerResponse = await client.PostAsync("http://localhost:52260/api/customers"
                , customerInfo);
            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }
            var getCustomerResponse=await client.GetAsync("http://localhost:52260/api/customers");
            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
                Console.ReadLine();
            }
            Console.ReadLine();

        }
    }
}
