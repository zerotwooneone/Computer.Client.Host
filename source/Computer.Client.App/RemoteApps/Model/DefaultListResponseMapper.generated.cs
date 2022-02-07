using Computer.Bus.Domain.Contracts.Models;
using System;

namespace Computer.Client.App.RemoteApps.Model;
#nullable enable
public class DefaultListResponseMapper : IMapper
{
    public object? DtoToDomain(Type dtoType, object obj, Type domainType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.DefaultListResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListResponse)) || obj == null)
        {
            return null;
        }

        var dto = obj as Computer.Client.App.RemoteApps.Model.DefaultListResponse;
        if (dto == null)
        {
            return null;
        }

        return DtoToDomain(dto);
    }

    public object? DomainToDto(Type domainType, object obj, Type dtoType)
    {
        if (!dtoType.IsAssignableFrom(typeof(Computer.Client.App.RemoteApps.Model.DefaultListResponse)) || !domainType.IsAssignableFrom(typeof(Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListResponse)) || obj == null)
        {
            return null;
        }

        var domain = obj as Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListResponse;
        if (domain == null)
        {
            return null;
        }

        return DomainToDto(domain);
    }

    public Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListResponse DtoToDomain(Computer.Client.App.RemoteApps.Model.DefaultListResponse dto)
    {
        return new Client.Domain.Contracts.Model.ToDoList.DefaultListResponse
        {
            Success = dto.Success,
            List = dto.List != null ? new Client.Domain.Contracts.Model.ToDoList.ListModel
            {
                Id = dto.List.Id,
                Items = dto.List.Items.Select(dtoListItem => new Client.Domain.Contracts.Model.ToDoList.ItemModel
                {
                    Text = dtoListItem.Text,
                    Url = dtoListItem.Url,
                    ImageUrl = dtoListItem.ImageUrl,
                    Checked = dtoListItem.Checked
                })
            } : null
        };
    }

    public Computer.Client.App.RemoteApps.Model.DefaultListResponse DomainToDto(Computer.Client.Domain.Contracts.Model.ToDoList.DefaultListResponse domain)
    {
        return new DefaultListResponse
        {
            Success = domain.Success,
            List = domain.List != null ? new ListModel
            {
                Id = domain.List.Id,
                Items = domain.List.Items.Select(domainListItem => new ItemModel
                {
                    Text = domainListItem.Text,
                    Url = domainListItem.Url,
                    ImageUrl = domainListItem.ImageUrl,
                    Checked = domainListItem.Checked
                })
            } : null
        };
    }
}