using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GitHubAutoresponder.Webhook {
    public class ModelStateConverter : IModelStateConverter {
        const string MESSAGE_HEADER = "Malformed payload:\n";

        public string AsString(ModelStateDictionary modelState) {
            StringBuilder builder = new StringBuilder(MESSAGE_HEADER);

            foreach (ModelStateEntry entry in modelState.Values) {
                builder.Append($"\n- {entry.Errors.FirstOrDefault().ErrorMessage}");
            }

            return builder.ToString();
        }
    }
}
