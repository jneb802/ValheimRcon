using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ValheimRcon.Tests
{
    [TestFixture]
    public class DiscordTests
    {
        [Test]
        public void Send_UsesHttpsWebhookAndIncludesPublicMessage()
        {
            RecordingHandler handler = new RecordingHandler();
            using HttpClient client = new HttpClient(handler);

            Discord.Send(client, "command output", "RCON", "https://discord.com/api/webhooks/id/token");

            Assert.AreEqual("https://discord.com/api/webhooks/id/token", handler.RequestUri);
            StringAssert.Contains("command output", handler.RequestBody);
            StringAssert.Contains("RCON", handler.RequestBody);
        }

        [Test]
        public void SendFile_IncludesCompleteTextFile()
        {
            RecordingHandler handler = new RecordingHandler();
            using HttpClient client = new HttpClient(handler);
            string filePath = Path.GetTempFileName();

            try
            {
                File.WriteAllText(filePath, "first line\nlast line");

                Discord.SendFile(
                    client,
                    "Full message",
                    "complete-output.txt",
                    filePath,
                    "RCON",
                    "https://discord.com/api/webhooks/id/token");

                StringAssert.Contains("Full message", handler.RequestBody);
                StringAssert.Contains("complete-output.txt", handler.RequestBody);
                StringAssert.Contains("first line\nlast line", handler.RequestBody);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        private sealed class RecordingHandler : HttpMessageHandler
        {
            public string RequestUri { get; private set; }
            public string RequestBody { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                RequestUri = request.RequestUri.ToString();
                RequestBody = await request.Content.ReadAsStringAsync();
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
    }
}
