using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpenAI
{
    public class Configuration
    {
        public Auth Auth { get; }
        
        /// For getting custom base path
        public class BasePathClass
        {
            public string BasePath { get; set; }
        }

        private BasePathClass basePathObj;
        public string BasePath => basePathObj.BasePath;
        
        /// Used for serializing and deserializing PascalCase request object fields into snake_case format for JSON. Ignores null fields when creating JSON strings.
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore, 
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CustomNamingStrategy()
            }
        };
        
        public Configuration(string apiKey = null, string organization = null, string basePath = "https://api.openai.com/v1")
        {
            if (apiKey == null)
            {
                var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var authPath = $"{userPath}/.openai/auth.json";

                if (File.Exists(authPath))
                {
                    var json = File.ReadAllText(authPath);
                    Auth = JsonConvert.DeserializeObject<Auth>(json, jsonSerializerSettings);
                    basePathObj = JsonConvert.DeserializeObject<BasePath>(json, jsonSerializerSettings);
                }
                else
                {
                    Debug.LogError("API Key is null and auth.json does not exist. Please check https://github.com/srcnalt/OpenAI-Unity#saving-your-credentials");
                }
            }
            else
            {
                Auth = new Auth()
                {
                    ApiKey = apiKey,
                    Organization = organization
                };
                BasePath = new BasePathClass()
                {
                    BasePath = basePath
                }
            }
        }
    }
}
