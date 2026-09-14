using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ValheimRcon
{
    static class Discord
    {
        private static readonly HttpClient Client = CreateClient();

        internal static void Send(string messageBody, string userName, string webhook)
        {
            Send(Client, messageBody, userName, webhook);
        }

        internal static void Send(HttpClient client, string messageBody, string userName, string webhook)
        {
            using MultipartFormDataContent content = CreateContent(messageBody, userName);
            Post(client, webhook, content);
        }

        internal static void SendFile(
            string messageBody,
            string fileName,
            string filePath,
            string userName,
            string webhook)
        {
            SendFile(Client, messageBody, fileName, filePath, userName, webhook);
        }

        internal static void SendFile(
            HttpClient client,
            string messageBody,
            string fileName,
            string filePath,
            string userName,
            string webhook)
        {
            using MultipartFormDataContent content = CreateContent(messageBody, userName);
            ByteArrayContent fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            content.Add(fileContent, "file", fileName);
            Post(client, webhook, content);
        }

        private static MultipartFormDataContent CreateContent(string messageBody, string userName)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StringContent(userName), "username");
            content.Add(new StringContent(messageBody), "content");
            return content;
        }

        private static void Post(HttpClient client, string webhook, HttpContent content)
        {
            using HttpResponseMessage response = client.PostAsync(webhook, content).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
        }

        private static HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("ValheimRcon/1.5.1");
            return client;
        }
    }
}
