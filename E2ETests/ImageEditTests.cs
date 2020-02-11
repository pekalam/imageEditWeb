using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using E2ETests.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace E2ETests
{
    public class ImageEditTests : IClassFixture<WebApplicationFactory<FakeStartup>>
    {
        private readonly HttpClient _client;

        public ImageEditTests()
        {
            var fx = new TestServerClientFixture();
            _client = fx.Client;
        }

        [Fact]
        public async Task ImageEditController_when_edit_action_called_returns_200_and_edit_task_consumer_receives_msg()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/edit");
            var content = new MultipartFormDataContent();


            content.Headers.Add("enctype", "multipart/form-data");

            var imageContent = new ByteArrayContent(File.ReadAllBytes(@"img/0.png"));


            imageContent.Headers.ContentType =
                new MediaTypeHeaderValue("image/png");
            content.Add(imageContent, "img", "0.jpg");
            content.Add(new StringContent("{\"opt1\": \"value1\"}"), "dict");

            request.Content = content;
            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(200);

//            lock (FakeEditTaskConsumer.lck)
//            {
//                Monitor.Pulse(FakeEditTaskConsumer.lck);
//                if (!Monitor.Wait(FakeEditTaskConsumer.lck, TimeSpan.FromSeconds(10)))
//                {
//                    Assert.True(false);
//                }
//            }
            Thread.Sleep(5000);

            Assert.True(FakeEditTaskConsumer.Called);
        }
    }
}