using System.Text.Json;
using SecondChance.Api.Presentation.Extensions;
using SecondChance.Application.Services;

namespace SecondChance.Api.Presentation.Services;

public class JsonSerializeService : IJsonSerializerService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public JsonSerializeService()
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
        _jsonSerializerOptions.SetApplicationJsonSerializeRules();
    }

    public string SerializeObject(object? value)
    {
        return JsonSerializer.Serialize(value, _jsonSerializerOptions);
    }

    public TObject? DeserializeObject<TObject>(string json)
    {
        return JsonSerializer.Deserialize<TObject>(json, _jsonSerializerOptions);
    }
}