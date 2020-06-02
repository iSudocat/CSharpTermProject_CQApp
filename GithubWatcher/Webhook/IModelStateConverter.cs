using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GitHubAutoresponder.Webhook {
    public interface IModelStateConverter {
        string AsString(ModelStateDictionary modelState);
    }
}
