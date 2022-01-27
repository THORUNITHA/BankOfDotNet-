using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace BankDotNet.IdentitySvr
{
    public class Config
    {
        /*public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }*/

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="Thorunitha",
                    Password="password"
                },
                new TestUser
                {
                    SubjectId="2",
                    Username="Thoru",
                    Password="password"
                }
            };
        }
        //Set up resource and client
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("BankOfDotNetApi","Customer Api for BankofDotNet")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            
            return new List<Client>
            {
                //Client-Credential based grant type
                new Client
                {
                    ClientId="client",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={ "BankOfDotNetApi" }
                },
                  //Resource Owner Password Grant type
                new Client
                {
                    ClientId="ro.client",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={"BankOfDotNetApi"}
                }
                //Implicit Flow Grant Type
              /*  new Client
                {
                    ClientId="mvc",
                    ClientName="MVC Client",
                    AllowedGrantTypes=GrantTypes.Implicit,
                    //User is redirected from the client to Identity server4
                    RedirectUris={"http://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris={"http://localhost:5003/signout-callback-oidc"},
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }*/
            };
        }
    }
}
