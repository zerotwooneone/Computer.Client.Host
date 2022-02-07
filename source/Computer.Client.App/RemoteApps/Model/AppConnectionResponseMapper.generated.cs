#nullable enable
using Computer.Bus.Domain.Contracts.Models;
using System;

namespace Computer.Client.App.RemoteApps.Model;
public partial class AppConnectionResponseMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.AppConnectionResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.AppConnectionResponse)) || obj == null)
        {
            return null;
        }

        var dto = (Computer.Client.App.RemoteApps.Model.AppConnectionResponse)obj;
        return new Computer.Client.Domain.Contracts.Model.AppConnectionResponse{InstanceId = dto.InstanceId};
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.AppConnectionResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.AppConnectionResponse)) || obj == null)
        {
            return null;
        }

        var domain = (Computer.Client.Domain.Contracts.Model.AppConnectionResponse)obj;
        return new Computer.Client.App.RemoteApps.Model.AppConnectionResponse{InstanceId = domain.InstanceId};
    }
}