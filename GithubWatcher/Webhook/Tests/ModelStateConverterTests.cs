using Xunit;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GitHubAutoresponder.Webhook.Tests {
    public class ModelStateConverterTests {
        private ModelStateConverter stateConverter;

        public ModelStateConverterTests() {
            this.stateConverter = new ModelStateConverter();
        }

        [Fact]
        public void ItShouldBuildAStringFromAModelStateDictionary() {
            ModelStateDictionary state = new ModelStateDictionary();

            state.AddModelError("one", "First error message");
            state.AddModelError("two", "Second error message");
            state.AddModelError("three", "Third error message");

            string expectedMessage = @"Malformed payload:

- First error message
- Second error message
- Third error message";

            string actualMessage = this.stateConverter.AsString(state);

            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}
