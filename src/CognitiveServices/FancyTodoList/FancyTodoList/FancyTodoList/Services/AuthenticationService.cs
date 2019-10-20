using FancyTodoList.Helpers;
using FancyTodoList.Interfaces;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FancyTodoList.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        string subscriptionKey;
        string token;
        string _authenticationTokenEndpoint;
        Timer accessTokenRenewer;
        const int RefreshTokenDuration = 9;
        HttpClient httpClient;

        public AuthenticationService(string apiKey)
        {
            subscriptionKey = apiKey;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
        }

        public async Task InitializeAsync(string authenticationTokenEndpoint)
        {
            token = await FetchTokenAsync(authenticationTokenEndpoint);
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }

        public string GetAccessToken()
        {
            return token;
        }

        async Task RenewAccessToken(string authenticationTokenEndpoint)
        {
            _authenticationTokenEndpoint = authenticationTokenEndpoint;

            token = await FetchTokenAsync(_authenticationTokenEndpoint);
            Debug.WriteLine("Renewed token.");
        }

        async Task OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                await RenewAccessToken(_authenticationTokenEndpoint);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Failed to renew access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        async Task<string> FetchTokenAsync(string fetchUri)
        {
            UriBuilder uriBuilder = new UriBuilder(fetchUri);
            uriBuilder.Path += "/issueToken";

            var result = await httpClient.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
            //var result = await httpClient.PostAsync(fetchUri, null);
            return await result.Content.ReadAsStringAsync();
        }
    }
}
