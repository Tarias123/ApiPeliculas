using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(20, ErrorMessage = "El nombre no puede exceder los 20 caracteres")]
        public string? Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
