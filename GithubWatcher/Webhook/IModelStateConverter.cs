using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GithubWatcher.Webhook {
    public interface IModelStateConverter {
        string AsString(ModelStateDictionary modelState);
    }
}
