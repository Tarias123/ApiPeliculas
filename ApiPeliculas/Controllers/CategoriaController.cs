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

        [HttpGet("{CatId:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int CatId)
        {
            var Categoria = _CatRepo.GetCategoria(CatId);
            if (Categoria == null)
            {
                return NotFound();
            }
            var CategoriaDto = _mapper.Map<CategoriaDto>(Categoria);
            return Ok(CategoriaDto);
        }

        [HttpPost("{CatId:int}", Name = "CreateCategoria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateCategoria([FromBody] CreateCategoriaDto crearcategoriaDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(crearcategoriaDto == null)
            {
                return BadRequest(ModelState);
            }
            if(_CatRepo.ExisteCategoria(crearcategoriaDto.Nombre!))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Models.Categoria>(crearcategoriaDto);
            if(!_CatRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { CatId = categoria.Id }, categoria);

        }

        [HttpPatch("{CatId:int}", Name = "ActualizarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int CatId, [FromBody] CategoriaDto actualizarcategoriaDto)
        {
            if (actualizarcategoriaDto == null || CatId != actualizarcategoriaDto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoria = _mapper.Map<Models.Categoria>(actualizarcategoriaDto);
            if (!_CatRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        [HttpPut("{CatId:int}", Name = "ActualizarPutCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPutCategoria(int CatId, [FromBody] CategoriaDto categoriaDto)
        {
            if (categoriaDto == null || CatId != categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }
            var categoriaExistente = _CatRepo.GetCategoria(CatId);
            if (categoriaExistente == null)
            {
                return NotFound($"No se encontro la categoria con el id {CatId}");
            }
            var categoria = _mapper.Map<Models.Categoria>(categoriaDto);
            if (!_CatRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }


        [HttpDelete("{CatId:int}", Name ="BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int CatId )
        {
            if (!_CatRepo.ExisteCategoria(CatId))
            {
                return NotFound();
            }
            var categoria = _CatRepo.GetCategoria(CatId);
            if (!_CatRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal a la hora de borrar la categoria {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
            
        


    }
}
