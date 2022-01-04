#nullable enable
using Computer.Bus.Domain.Contracts;
using System;

namespace Computer.Client.Host.App.Model;
public partial class AppConnectionResponseMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.Host.App.Model.AppConnectionResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Model.AppConnectionResponse)) || obj == null)
        {
            return null;
        }

        var dto = (Computer.Client.Host.App.Model.AppConnectionResponse)obj;
        return new Computer.Client.Domain.Model.AppConnectionResponse{InstanceId = dto.InstanceId};
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.Host.App.Model.AppConnectionResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Model.AppConnectionResponse)) || obj == null)
        {
            return null;
        }

        var domain = (Computer.Client.Domain.Model.AppConnectionResponse)obj;
        return new Computer.Client.Host.App.Model.AppConnectionResponse{InstanceId = domain.InstanceId};
    }
}