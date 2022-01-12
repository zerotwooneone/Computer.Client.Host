using System.Text.Json;

namespace Computer.Client.App.Bus;

public record SubjectConfig
{
    public readonly Type? type = null;
    public readonly Func<Type, JsonElement, object>? ConvertFromHub = null;

    public SubjectConfig()
    {
    }

    public SubjectConfig(Type type, Func<Type, JsonElement, object> Convert)
    {
        this.type = type;
        ConvertFromHub = Convert;
    }
}