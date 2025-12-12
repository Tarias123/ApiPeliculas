using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.ItRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _CatRepo;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaRepository CatRepo, IMapper mapper)
        {
            _CatRepo = CatRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _CatRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();
            foreach (var categoria in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(categoria));
            }
            return Ok(listaCategoriasDto);
        }


    }
}
