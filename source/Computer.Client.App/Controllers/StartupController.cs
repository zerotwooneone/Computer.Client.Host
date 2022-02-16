using Computer.Client.Domain.Contracts.App.ToDoList;
using Computer.Client.Domain.Contracts.Model.ToDoList;
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
        var list = await _listService.GetDefaultListByUserId("some user id").ConfigureAwait(false);
        if (list.Success)
        {
            if (list.Left == null)
            {
                throw new Exception("Request was not a success");
            }
            return Json(new StartupModel(list.Left.Value));
        }

        return BadRequest("user id was incorrect");
    }
}

public record StartupParam();
public record StartupModel(ListModel Default);