using Moq;
using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GitHubAutoresponder.Shared;
using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder.Tests {
    public class GitHubResponderTests {
        private Mock<IResponseFactory> responseFactory;
        private Mock<IHttpClient> httpClient;
        private Mock<IJsonSerialiser> jsonSerialiser;
        private IGitHubResponder gitHubResponder;

        public GitHubResponderTests() {
            this.responseFactory = new Mock<IResponseFactory>();
            this.httpClient = new Mock<IHttpClient>();
            this.jsonSerialiser = new Mock<IJsonSerialiser>();

            this.gitHubResponder = new GitHubResponder(
                responseFactory.Object,
                this.httpClient.Object,
                this.jsonSerialiser.Object
            );
        }

        [Fact]
        public async Task ItShouldReturnTrueWhenTheRequestSucceeds() {
            string url = "https://foobar";

            Payload payload = new Payload {
                Action = "opened",

                Issue = new Commentable {
                    CommentsUrl = url
                }
            };

            Response response = new Response("foo");

            this.responseFactory
                .Setup(r => r.CreateFromPayload(payload))
                .Returns(response);

            this.jsonSerialiser
                .Setup(j => j.Serialise(response))
                .Returns("serialised JSON");

            this.httpClient
                .Setup(h => h.PostAsync(url, "serialised JSON"))
                .ReturnsAsync(true);

            bool result = await this.gitHubResponder.RespondAsync(payload);

            Assert.True(result);
        }

        [Fact]
        public async Task ItShouldReturnTrueWhenThePayloadActionIsNotOpened() {
            string url = "https://foobar";

            Payload payload = new Payload {
                Action = "comment",

                Issue = new Commentable {
                    CommentsUrl = url
                }
            };

            Response response = new Response("foo");

            this.responseFactory
                .Setup(r => r.CreateFromPayload(payload))
                .Returns(response);

            this.jsonSerialiser
                .Setup(j => j.Serialise(response))
                .Returns("serialised JSON");

            this.httpClient
                .Setup(h => h.PostAsync(url, "serialised JSON"))
                .ReturnsAsync(true);

            bool result = await this.gitHubResponder.RespondAsync(payload);

            Assert.True(result);
            this.httpClient.Verify(h => h.PostAsync(url, "serialised JSON"), Times.Never());
            this.responseFactory.Verify(r => r.CreateFromPayload(payload), Times.Never());
            this.jsonSerialiser.Verify(j => j.Serialise(response), Times.Never());
        }

        [Fact]
        public async Task ItShouldReturnFalseWhenTheRequestFails() {
            string url = "https://foobar";

            Payload payload = new Payload {
                Action = "opened",

                Issue = new Commentable {
                    CommentsUrl = url
                }
            };

            Response response = new Response("foo");

            this.responseFactory
                .Setup(r => r.CreateFromPayload(payload))
                .Returns(response);

            this.jsonSerialiser
                .Setup(j => j.Serialise(response))
                .Returns("serialised JSON");

            this.httpClient
                .Setup(h => h.PostAsync(url, "serialised JSON"))
                .ReturnsAsync(false);

            bool result = await this.gitHubResponder.RespondAsync(payload);

            Assert.False(result);
        }
    }
}
