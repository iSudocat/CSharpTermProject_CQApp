using System;
using System.IO;
using Newtonsoft.Json;

namespace GitHubAutoresponder.Shared {
    public class JsonSerialiser : IJsonSerialiser {
        private JsonSerializerSettings settings;

        public JsonSerialiser() {
            this.settings = new JsonSerializerSettings {
                ContractResolver = JsonContractResolver.Resolver
            };
        }

        public string Serialise(object obj) {
            return JsonConvert.SerializeObject(obj, this.settings);
        }

        public T Deserialise<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json, this.settings);
        }
    }
}
