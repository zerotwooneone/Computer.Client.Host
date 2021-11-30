using Computer.Client.Domain.Contracts.ToDoList;
using Microsoft.AspNetCore.Mvc;

namespace Computer.Client.Host.Controllers;

public class ToDoListController : Controller
{
    private readonly IListService listService;

    public ToDoListController(IListService listService)
    {
        this.listService = listService;
    }
    [HttpGet]
    public async Task<IActionResult> Get(string id)
    {
        var list = await listService.GetById(id);
        return Ok(list);
    }

    [HttpPut]
    public async Task<IActionResult> Put(string id, [FromBody] DocumentModel document)
    {
        return Ok(id);
    }
}
