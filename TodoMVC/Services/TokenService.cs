using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TodoMVC.Models;

namespace TodoMVC.Services
{
    public class TokenService : ITokenService
    {
        private AuthenticationResult authResult;
        private readonly IOptions<AzureAD> azureAd;
        public TokenService(IOptions<AzureAD> _azureAd)
        {
            azureAd = _azureAd;

        }
        public async Task<string> GetToken()
        {
            if (authResult != null && authResult.ExpiresOn > DateTime.UtcNow)
            {
                return authResult.AccessToken;
            }
            else
            {
                string aadInstance = azureAd.Value.Instance;
                string tenant = azureAd.Value.TenantId;
                string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
                var authContext = new AuthenticationContext(authority);
                var clientCredential = new ClientCredential(azureAd.Value.ClientId, azureAd.Value.ClientSecret);
                authResult = await authContext.AcquireTokenAsync(azureAd.Value.ResourceId, clientCredential);
                return authResult.AccessToken;
            }
            
        }
    }
}
