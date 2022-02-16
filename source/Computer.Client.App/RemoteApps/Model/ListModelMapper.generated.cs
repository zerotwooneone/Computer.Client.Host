using Computer.Bus.Domain.Contracts.Models;
using System;

namespace Computer.Client.App.RemoteApps.Model;
#nullable enable
public class ListModelMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListModel)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListModel)) || obj == null)
        {
            return null;
        }

        var dto = obj as Computer.Client.App.RemoteApps.Model.ListModel;
        if (dto == null)
        {
            return null;
        }

        return DtoToDomain(dto);
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.ListModel)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.ListModel)) || obj == null)
        {
            return null;
        }

        var domain = obj as Computer.Client.Domain.Contracts.Model.ToDoList.ListModel;
        if (domain == null)
        {
            return null;
        }

        return DomainToDto(domain);
    }

    public Computer.Client.Domain.Contracts.Model.ToDoList.ListModel DtoToDomain(Computer.Client.App.RemoteApps.Model.ListModel dto)
    {
        return new Client.Domain.Contracts.Model.ToDoList.ListModel
        {
            Id = dto.Id,
            Items = dto.Items.Select(dtoItem => new Client.Domain.Contracts.Model.ToDoList.ItemModel
            {
                Text = dtoItem.Text,
                Url = dtoItem.Url,
                ImageUrl = dtoItem.ImageUrl,
                Checked = dtoItem.Checked
            })
        };
    }

    public Computer.Client.App.RemoteApps.Model.ListModel DomainToDto(Computer.Client.Domain.Contracts.Model.ToDoList.ListModel domain)
    {
        return new ListModel
        {
            Id = domain.Id,
            Items = domain.Items.Select(domainItem => new ItemModel
            {
                Text = domainItem.Text,
                Url = domainItem.Url,
                ImageUrl = domainItem.ImageUrl,
                Checked = domainItem.Checked
            }).ToList()
        };
    }
}