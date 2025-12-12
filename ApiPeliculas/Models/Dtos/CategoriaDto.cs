using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CreateCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(20, ErrorMessage = "El nombre no puede exceder los 20 caracteres")]
        public string? Nombre { get; set; }

    }
}
