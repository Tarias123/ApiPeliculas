using ApiPeliculas.Data;
using ApiPeliculas.Models;
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
        private readonly ICategoriaRepository _catrepo;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaRepository catrepo, IMapper mapper)
        {
            _catrepo = catrepo;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCategorias()
        {
            var categorias = _catrepo.GetCategorias();
            var categoriasDto = _mapper.Map<List<Models.Dtos.CategoriaDto>>(categorias);
            return Ok(categoriasDto);
        }



        [HttpGet("{categoriaId:int}", Name = "GetCategoriaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCategoriaId(int categoriaId)
        {
            var categoria = _catrepo.GetCategoria(categoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDto = _mapper.Map<Models.Dtos.CategoriaDto>(categoria);
            return Ok(categoriaDto);
        }

        [HttpGet("GetCategoriaPorNombre/{nombre}", Name = "GetCategoriaNombre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCategoriaNombre(string nombre)
        {
            var categoria = _catrepo.GetCategoriaPorNombre(nombre);
            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDto = _mapper.Map<Models.Dtos.CategoriaDto>(categoria);
            return Ok(categoriaDto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public IActionResult CrearCategoria([FromBody] CreateCategoriaDto categoriaDtoEntrada)
        {

            if (categoriaDtoEntrada == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_catrepo.CategoriaExiste(categoriaDtoEntrada.Nombre!))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(StatusCodes.Status409Conflict, ModelState); 
            }

            var categoria = _mapper.Map<Categoria>(categoriaDtoEntrada);

            if (!_catrepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", "Error a la hora de crear categoria");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }


        [HttpPut("{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Añadido 404 para documentación
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto) // <-- CORRECCIÓN AQUI
        {
            // Validación 400
            if (categoriaDto == null || categoriaId != categoriaDto.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validación 404
            if (!_catrepo.CategoriaExisteId(categoriaId))
            {
                return NotFound();
            }

            // Mapeo y Actualización
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (!_catrepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $" Error al querer actualizar la categoria {categoria.Id}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

    }
}