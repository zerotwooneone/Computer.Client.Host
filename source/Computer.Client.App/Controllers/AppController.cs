using Computer.Client.App.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;

namespace Computer.Client.App.Controllers;

[Route("[Controller]/{service:length(1,512)}/{method:length(1,512)}.[Action]")]
public class AppController : Controller
{
    private readonly IAppService appService;

    public AppController(IAppService appService)
    {
        this.appService = appService;
    }

    [HttpPost]
    public async Task<IActionResult> Json(string service, string method, [FromBody] dynamic body)
    {
        try
        {
            var json = body.ToString(); //this is kind of hacky, and depends upon the implementation of the dynamic type
            var result = await appService.JsonFunction(service, method, json);
            return Content(result, "application/json");
        }
        catch (RuntimeBinderException)
        {
            return ValidationProblem();
        }
    }
}