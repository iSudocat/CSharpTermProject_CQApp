using Xunit;
using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder.Tests {
    public class ResponseFactoryTests {
        [Fact]
        public void ItShouldInstantiateAResponseWithAMessage() {
            ResponseFactory factory = new ResponseFactory();

            Payload payload = new Payload {
                Sender = new Sender {
                    Login = "Bob"
                },

                Repository = new Repository {
                    Name = "some-magic-repo"
                }
            };

            string expectedMessage = @"
Hi @Bob,

Thanks for your contribution to some-magic-repo! I am currently travelling, so I will not be able to address this until I return; this is merely an automated response. I apologise for the inconvenience and thank you for your patience.

Best wishes,
James
            ";

            Response response = factory.CreateFromPayload(payload);

            Assert.Equal(expectedMessage, response.Body);
        }
    }
}
