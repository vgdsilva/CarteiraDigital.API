using AutoMapper;
using CarteiraDigital.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers
{
    [ApiController]
    public partial class BaseApiController : ControllerBase
    {
        protected IMapper Mapper => MapperService.CreateMapper();
    }
}
