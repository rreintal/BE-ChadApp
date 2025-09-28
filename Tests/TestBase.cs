using System.Text.Json;

namespace Tests;

public abstract class TestBase
{
    protected T GetDto<T>(string name)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", typeof(T).Name, $"{name}.json");
        var json = File.ReadAllText(path);
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        return JsonSerializer.Deserialize<T>(json, options)!;
    }
}