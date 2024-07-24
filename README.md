[![Twitter](https://img.shields.io/twitter/url/https/twitter.com/sgt3v.svg?style=social&label=Follow%20%40sgt3v)](https://twitter.com/sgt3v)

Edited by 323 Studios for use in Sojourner

## OpenAI Unity Package
An unofficial Unity package that allows you to use the OpenAI API directly in the Unity game engine.

### Importing the Package
To import the package, follow these steps:
- Open Unity 2019 or later
- Go to `Window > Package Manager`
- Click the `+` button and select `Add package from git URL`
- Paste the repository URL https://github.com/sumomomomomo/OpenAI-Unity-local.git and click `Add`

### Saving Your Credentials
To make requests to the OpenAI API, you need to use your API key, and provide a url to the path of the OpenAI API provider.

To do this, follow these steps:

- Create a folder called .openai in your home directory (e.g. `C:User\UserName\` for Windows or `~\` for Linux or Mac)
- Create a file called `auth.json` in the `.openai` folder
- Add an api_key field to the auth.json file and save it (If you are here for testing our game, use "orbital" as the api_key without quotation marks. Remember to use the url we provide in the readme)
- Add a base_path to the Chat Completions API
- Here is an example of what your auth.json file should look like:

```json
{
    "api_key": "orbital",
    "base_path": "http://127.0.0.1:5000/v1"
}
```

**IMPORTANT:** Your API key is a secret. 
Do not share it with others or expose it in any client-side code (e.g. browsers, apps). 
If you are using OpenAI for production, make sure to run it on the server side, where your API key can be securely loaded from an environment variable or key management service.

### Making Requests to OpenAPI
You can use the `OpenAIApi` class to make async requests to the OpenAI API.

All methods are asynchronous and can be accessed directly from an instance of the `OpenAIApi` class.

Here is an example of how to make a request:

```csharp
private async void SendRequest()
{
    var req = new CreateChatCompletionRequest
    {
        Model = "gpt-3.5-turbo",
        Messages = new List<ChatMessage>()
        {
            new ChatMessage()
            {
                Role = "user",
                Content = "Hello!"
            }
        }
    };
    var res = await openai.CreateChatCompletion(req);
}
```

To make a stream request, you can use the `CreateCompletionAsync` and `CreateChatCompletionAsync` methods. 
These methods are going to set `Stream` property of the request to `true` and return responses through an onResponse callback.
In this case text responses are stored in `Delta` property of the `Choices` field in the Chat Completion.

```csharp
var req = new CreateChatCompletionRequest{
    Model = "gpt-3.5-turbo",
    Messages = new List<ChatMessage>
    {
        new ChatMessage()
        {
            Role = "user",
            Content = "Write a 100 word long short story in La Fontaine style."
        }
    },
    Temperature = 0.7f,
};
    
openai.CreateChatCompletionAsync(req, 
    (responses) => {
        var result = string.Join("", responses.Select(response => response.Choices[0].Delta.Content));
        Debug.Log(result);
    }, 
    () => {
        Debug.Log("completed");
    }, 
    new CancellationTokenSource()
);
```

### Sample Projects
This package includes two sample scenes that you can import via the Package Manager:

- **ChatGPT sample:** A simple ChatGPT like chat example.
- **DallE sample:** A DALL.E text to image generation example.

### Known Issues
- **Can't See the Image Result in WebGL Builds:** Due to CORS policy of OpenAI image storage in local WebGL builds you will get the generated image's URL however it will not be
downloaded using UnityWebRequest until you run it out of localhost, on a server.

- **Streamed Response is just blank in WebGL Build:** Unity 2020 WebGL has a bug where stream responses return empty. You can update and try with a newer version of Unity.

### Supported Unity Versions for WebGL Builds
The following table shows the supported Unity versions for WebGL builds: 

| Unity Version | Windows | Linux | MacOS | WebGL | Android | IOS | Oculus 2|
| --- | --- | --- | --- | --- | --- | --- | --- |
| 2022.3.x | ✅ | ⚠️ | ⚠️ | ⚠️ | ✅ | ✅ | ✅ | ✅ |
| 2021.3.x | ✅ | ⚠️ | ⚠️ | ⚠️ | ✅ | ✅ | ✅ | ⚠️ |
| 2020.3.x | ✅ | ⚠️ | ⚠️ | ⚠️ | ✅ | ✅ | ✅ | ⚠️ |
| 2019.4.x | ✅ | ⚠️ | ⚠️ | ⚠️ | ⚠️ | ⚠️ | ⚠️ | ⚠️ |

|  |  |  |
| --- | --- | --- |
| ✅ = Supported | ⚠️ = Not Tested | ⛔ = Not Supported |

Please report any issues you encounter with builds.

### Further Reading
For more information on how to use the various request parameters, please refer to the OpenAI documentation: https://platform.openai.com/docs/api-reference
