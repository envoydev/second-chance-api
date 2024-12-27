namespace SecondChance.Application.Services;

public interface IJsonSerializerService
{
    string SerializeObject(object? value);
    TObject? DeserializeObject<TObject>(string json);
}