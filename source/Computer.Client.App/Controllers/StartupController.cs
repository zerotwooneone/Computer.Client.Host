using Computer.Client.Domain.Contracts.App.ToDoList;
using Computer.Client.Domain.Contracts.Model;
using Microsoft.AspNetCore.Mvc;

namespace Computer.Client.App.Controllers;

[Route("Startup")]
public class StartupController : Controller
{
    private readonly IListService _listService;

    public StartupController(IListService listService)
    {
        _listService = listService;
    }
    public async Task<IActionResult> Post(StartupParam param)
    {
        var list = await _listService.GetDefaultListByUserId("some user id");
        if (list.Success)
        {
            return Json(new StartupModel(list.Left.Value));
        }

        return BadRequest("user id was incorrect");
    }
}

public record StartupParam();
public record StartupModel(ListModel Default);