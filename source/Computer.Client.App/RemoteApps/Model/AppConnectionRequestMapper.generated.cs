#nullable enable
using System;
using Computer.Bus.Domain.Contracts.Models;

namespace Computer.Client.App.RemoteApps.Model;
public partial class AppConnectionRequestMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.AppConnectionRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.AppConnectionRequest)) || obj == null)
        {
            return null;
        }

        var dto = (Computer.Client.App.RemoteApps.Model.AppConnectionRequest)obj;
        return new Computer.Client.Domain.Contracts.Model.AppConnectionRequest{InstanceId = dto.InstanceId, AppId = dto.AppId};
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.AppConnectionRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.AppConnectionRequest)) || obj == null)
        {
            return null;
        }

        var domain = (Computer.Client.Domain.Contracts.Model.AppConnectionRequest)obj;
        return new Computer.Client.App.RemoteApps.Model.AppConnectionRequest{InstanceId = domain.InstanceId, AppId = domain.AppId};
    }
}