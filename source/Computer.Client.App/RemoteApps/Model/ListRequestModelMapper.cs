using Computer.Bus.Domain.Contracts.Models;

namespace Computer.Client.App.RemoteApps.Model;

public class ListRequestModelMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListRequest)) || obj == null)
        {
            return null;
        }

        var dto = obj as Computer.Client.App.RemoteApps.Model.ListRequest;
        if (dto == null || string.IsNullOrWhiteSpace(dto.Message))
        {
            return null;
        }

        return DtoToDomain(dto);
    }

    private Computer.Client.Domain.Contracts.Model.ToDoList.ListRequest DtoToDomain(Computer.Client.App.RemoteApps.Model.ListRequest dtoType)
    {
        return new Client.Domain.Contracts.Model.ToDoList.ListRequest
        {
            Message = dtoType.Message
        };
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListRequest)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListRequest)) || obj == null)
        {
            return null;
        }

        var domain = obj as Computer.Client.Domain.Contracts.Model.ToDoList.ListRequest;
        if (domain == null || string.IsNullOrWhiteSpace(domain.Message))
        {
            return null;
        }

        return DomainToDto(domain);
    }

    private Computer.Client.App.RemoteApps.Model.ListRequest DomainToDto(Client.Domain.Contracts.Model.ToDoList.ListRequest domainType)
    {
        return new ListRequest()
        {
            Message = domainType.Message
        };
    }
}