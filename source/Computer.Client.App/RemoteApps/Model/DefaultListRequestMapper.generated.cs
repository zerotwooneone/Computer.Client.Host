using Computer.Bus.Domain.Contracts.Models;
using System;

namespace Computer.Client.App.RemoteApps.Model;
#nullable enable
public class DefaultListRequestMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.DefaultListRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListRequest)) || obj == null)
        {
            return null;
        }

        var dto = obj as Computer.Client.App.RemoteApps.Model.DefaultListRequest;
        if (dto == null)
        {
            return null;
        }

        return DtoToDomain(dto);
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.DefaultListRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListRequest)) || obj == null)
        {
            return null;
        }

        var domain = obj as Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListRequest;
        if (domain == null)
        {
            return null;
        }

        return DomainToDto(domain);
    }

    public Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListRequest DtoToDomain(Computer.Client.App.RemoteApps.Model.DefaultListRequest dto)
    {
        return new Client.Domain.Contracts.Model.ToDoList.DefaultListRequest
        {
            UserId = dto.UserId
        };
    }

    public Computer.Client.App.RemoteApps.Model.DefaultListRequest DomainToDto(Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListRequest domain)
    {
        return new DefaultListRequest
        {
            UserId = domain.UserId
        };
    }
}