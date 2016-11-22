﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mews.Eet.Communication
{
    public class SoapHttpClient
    {
        public SoapHttpClient(Uri endpointUri)
        {
            EndpointUri = endpointUri;
            HttpClient = new HttpClient();
            EnableTls12();
        }

        private Uri EndpointUri { get; }

        private HttpClient HttpClient { get; }

        public async Task<string> SendAsync(string body, string operation, TimeSpan timeout)
        {
            HttpClient.DefaultRequestHeaders.Remove("SOAPAction");
            HttpClient.DefaultRequestHeaders.Add("SOAPAction", operation);
            HttpClient.Timeout = timeout;

            var requestContent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            using (var response = await HttpClient.PostAsync(EndpointUri, requestContent).ConfigureAwait(continueOnCapturedContext: false))
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        private static void EnableTls12()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
    }
}