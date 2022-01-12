using Computer.Bus.Domain.Contracts;

namespace Computer.Client.App.Domain;

public class MapperFactory : IMapperFactory
{
    private readonly DomainMapRegistrationService _domainMapRegistrationService;

    public MapperFactory(DomainMapRegistrationService domainMapRegistrationService)
    {
        _domainMapRegistrationService = domainMapRegistrationService;
    }

    public IMapper? GetMapper(Type mapperType, Type dto, Type domain)
    {
        if (!_domainMapRegistrationService.TypeToInstance.TryGetValue(mapperType, out var mapper)) return null;

        return mapper;
    }
}