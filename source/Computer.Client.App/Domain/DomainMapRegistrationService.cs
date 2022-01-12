using Computer.Bus.Domain.Contracts;

namespace Computer.Client.App.Domain;

public class DomainMapRegistrationService
{
    public IReadOnlyDictionary<Type, IMapper> TypeToInstance => _typeToInstance;
    private readonly Dictionary<Type, IMapper> _typeToInstance = new();

    /// <summary>
    /// Adds or replaces an existing registration
    /// </summary>
    /// <param name="mapperTypes"></param>
    public void Register(IEnumerable<Type> mapperTypes)
    {
        foreach (var mapperType in mapperTypes)
        {
            var obj = Activator.CreateInstance(mapperType);
            if (obj is not IMapper mapper) continue;
            _typeToInstance[mapperType] = mapper;
        }
    }
}