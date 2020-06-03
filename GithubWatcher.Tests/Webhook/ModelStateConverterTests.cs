using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GithubWatcher.Webhook.Tests {
    [TestClass]
    public class ModelStateConverterTests {
        private ModelStateConverter stateConverter;

        public ModelStateConverterTests() {
            this.stateConverter = new ModelStateConverter();
        }

        [TestMethod]
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

            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
