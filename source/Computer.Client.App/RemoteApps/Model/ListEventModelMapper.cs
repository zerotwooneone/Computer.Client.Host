using Computer.Bus.Domain.Contracts.Models;

namespace Computer.Client.App.RemoteApps.Model;

public class ListEventModelMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListEvent)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListEvent)) || obj == null)
        {
            return null;
        }

        var dto = obj as Computer.Client.App.RemoteApps.Model.ListEvent;
        if (dto == null || string.IsNullOrWhiteSpace(dto.Message))
        {
            return null;
        }

        return DtoToDomain(dto);
    }

    private Computer.Client.Domain.Contracts.Model.ToDoList.ListEvent DtoToDomain(Computer.Client.App.RemoteApps.Model.ListEvent dtoType)
    {
        return new Client.Domain.Contracts.Model.ToDoList.ListEvent
        {
            Message = dtoType.Message
        };
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListEvent)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListEvent)) || obj == null)
        {
            return null;
        }

        var domain = obj as Computer.Client.Domain.Contracts.Model.ToDoList.ListEvent;
        if (domain == null || string.IsNullOrWhiteSpace(domain.Message))
        {
            return null;
        }

        return DomainToDto(domain);
    }

    private Computer.Client.App.RemoteApps.Model.ListEvent DomainToDto(Client.Domain.Contracts.Model.ToDoList.ListEvent domainType)
    {
        return new ListEvent()
        {
            Message = domainType.Message
        };
    }
}