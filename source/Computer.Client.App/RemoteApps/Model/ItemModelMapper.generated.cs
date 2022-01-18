using Computer.Bus.Domain.Contracts;
using System;

namespace Computer.Client.App.RemoteApps.Model;
#nullable enable
public class ItemModelMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ItemModel)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ItemModel)) || obj == null)
        {
            return null;
        }

        var dto = obj as Computer.Client.App.RemoteApps.Model.ItemModel;
        if (dto == null)
        {
            return null;
        }

        return DtoToDomain(dto);
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ItemModel)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ItemModel)) || obj == null)
        {
            return null;
        }

        var domain = obj as Computer.Client.Domain.Contracts.Model.ToDoList.ItemModel;
        if (domain == null)
        {
            return null;
        }

        return DomainToDto(domain);
    }

    public Computer.Client.Domain.Contracts.Model.ToDoList.ItemModel DtoToDomain(Computer.Client.App.RemoteApps.Model.ItemModel dto)
    {
        return new Client.Domain.Contracts.Model.ToDoList.ItemModel
        {
            Text = dto.Text,
            Url = dto.Url,
            ImageUrl = dto.ImageUrl,
            Checked = dto.Checked
        };
    }

    public Computer.Client.App.RemoteApps.Model.ItemModel DomainToDto(Computer.Client.Domain.Contracts.Model.ToDoList.ItemModel domain)
    {
        return new ItemModel
        {
            Text = domain.Text,
            Url = domain.Url,
            ImageUrl = domain.ImageUrl,
            Checked = domain.Checked
        };
    }
}