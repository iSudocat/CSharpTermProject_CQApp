using System.IO;

namespace GitHubAutoresponder.Shared {
    public interface IJsonSerialiser {
        string Serialise(object obj);
        T Deserialise<T>(string json);
    }
}
