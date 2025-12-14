using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; } 
        public string RutaImagen { get; set; }
        public DateTime FechaEstreno { get; set; }
        public enum TipoClasificacion
        {
            siete,
            doce,
            quince,
            dieciocho
        }
        public TipoClasificacion clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        //relaciones con categoria
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

    }
}
