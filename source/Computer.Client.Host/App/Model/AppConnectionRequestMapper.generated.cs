#nullable enable
using Computer.Bus.Domain.Contracts;
using System;

namespace Computer.Client.Host.App.Model;
public partial class AppConnectionRequestMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.Host.App.Model.AppConnectionRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Model.AppConnectionRequest)) || obj == null)
        {
            return null;
        }

        var dto = (Computer.Client.Host.App.Model.AppConnectionRequest)obj;
        return new Computer.Client.Domain.Model.AppConnectionRequest{InstanceId = dto.InstanceId, AppId = dto.AppId};
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.Host.App.Model.AppConnectionRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Model.AppConnectionRequest)) || obj == null)
        {
            return null;
        }

        var domain = (Computer.Client.Domain.Model.AppConnectionRequest)obj;
        return new Computer.Client.Host.App.Model.AppConnectionRequest{InstanceId = domain.InstanceId, AppId = domain.AppId};
    }
}