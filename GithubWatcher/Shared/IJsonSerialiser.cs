using System.IO;

namespace GithubWatcher.Shared {
    public interface IJsonSerialiser {
        string Serialise(object obj);
        T Deserialise<T>(string json);
    }
}
