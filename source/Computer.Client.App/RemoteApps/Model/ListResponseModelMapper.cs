using Computer.Bus.Domain.Contracts.Models;

namespace Computer.Client.App.RemoteApps.Model;

public class ListResponseModelMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListResponse)) || obj == null)
        {
            return null;
        }

        var dto = obj as Computer.Client.App.RemoteApps.Model.ListResponse;
        if (dto == null || string.IsNullOrWhiteSpace(dto.Message))
        {
            return null;
        }

        return DtoToDomain(dto);
    }

    private Computer.Client.Domain.Contracts.Model.ToDoList.ListResponse DtoToDomain(Computer.Client.App.RemoteApps.Model.ListResponse dtoType)
    {
        return new Client.Domain.Contracts.Model.ToDoList.ListResponse
        {
            Message = dtoType.Message
        };
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListResponse)) || obj == null)
        {
            return null;
        }

        var domain = obj as Computer.Client.Domain.Contracts.Model.ToDoList.ListResponse;
        if (domain == null || string.IsNullOrWhiteSpace(domain.Message))
        {
            return null;
        }

        return DomainToDto(domain);
    }

    private Computer.Client.App.RemoteApps.Model.ListResponse DomainToDto(Client.Domain.Contracts.Model.ToDoList.ListResponse domainType)
    {
        return new ListResponse()
        {
            Message = domainType.Message
        };
    }
}