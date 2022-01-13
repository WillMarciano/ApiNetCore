using MarcianoIO.Api.Controllers;
using MarcianoIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarcianoIO.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        public TesteController(INotificador notificador, IUser appUser) : base(notificador, appUser)
        {
        }

        [HttpGet]
        public string RetornaValor()
        {
            return "Version V2";
        }
    }
}
