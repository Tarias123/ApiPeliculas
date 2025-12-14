using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.ItRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiPeliculas.Models;
using Microsoft.AspNetCore.Http.HttpResults; // Necesario para referenciar la entidad Pelicula

namespace ApiPeliculas.Controllers
{
    // Ruta base para el controlador
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepository _PeliculaRepo;
        private readonly IMapper _mapper;

        // Inyección de Dependencias
        public PeliculasController(IPeliculaRepository PeliculaRepo, IMapper mapper)
        {
            _PeliculaRepo = PeliculaRepo;
            _mapper = mapper;
        }

        // --- GET: Listar todas las películas ---
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _PeliculaRepo.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();

            // Mapeo manual de la lista de Entidades a DTOs
            foreach (var pelicula in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(pelicula));
            }
            return Ok(listaPeliculasDto);
        }

        // --- GET: Obtener película por ID ---
        // Se corrigió la ambigüedad: el nombre de la variable coincide con el de la ruta
        [HttpGet("{PeliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPelicula(int PeliculaId) // Corregido: Usar PeliculaId
        {
            var Pelicula = _PeliculaRepo.GetPelicula(PeliculaId);
            if (Pelicula == null)
            {
                return NotFound();
            }
            var PeliculaDto = _mapper.Map<PeliculaDto>(Pelicula);
            return Ok(PeliculaDto);
        }

        [HttpPost] 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Se agrega si CategoriaId podría ser inválido
        public IActionResult CreatePelicula([FromBody] CreatePeliculaDto crearPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearPeliculaDto == null)
            {
                return BadRequest(crearPeliculaDto);
            }

            // Validación de existencia por nombre
            if (_PeliculaRepo.ExistePelicula(crearPeliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(400, ModelState);
            }

            // Mapear DTO de entrada a la Entidad
            var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);

            if (!_PeliculaRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            // Retorna 201 Created con la URL para obtener el nuevo recurso
            return CreatedAtRoute("GetPelicula", new { PeliculaId = pelicula.Id }, pelicula);
        }


        [HttpPatch("{PeliculaId:int}", Name = "ActualizarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPelicula(int PeliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (peliculaDto == null || PeliculaId != peliculaDto.Id)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var peliculaExistente = _PeliculaRepo.GetPelicula(PeliculaId);
            if (peliculaExistente == null)
            {
                return NotFound($"No se encontro la pelicula con el id {peliculaExistente}");
            }
            var peliculaMapeada = _mapper.Map<Pelicula>(peliculaDto);

            if (!_PeliculaRepo.ActualizarPelicula(peliculaMapeada))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {peliculaMapeada.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }



        // --- DELETE: Borrar película por ID ---
        [HttpDelete("{PeliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarPelicula(int PeliculaId) // Corregido: Usar PeliculaId
        {
            if (!_PeliculaRepo.ExistePelicula(PeliculaId))
            {
                return NotFound();
            }

            var pelicula = _PeliculaRepo.GetPelicula(PeliculaId);

            if (!_PeliculaRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listapeliculas = _PeliculaRepo.GetPeliculasEnCategoria(categoriaId);
            if(listapeliculas == null)
            {
                return NotFound();
            }
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach(var pelicula in listapeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(pelicula));
            }

            return Ok(listaPeliculasDto);
        }

        [HttpGet("BuscarPorNombre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarPorNombre([FromQuery] string nombre)
        {
            try
            {
                var resultado = _PeliculaRepo.BuscarPelicula(nombre);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound("No se encontraron películas que coincidan con el nombre proporcionado.");

            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}