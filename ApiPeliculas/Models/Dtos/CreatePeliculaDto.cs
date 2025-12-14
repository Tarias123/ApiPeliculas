namespace ApiPeliculas.Models.Dtos
{
    public class CreatePeliculaDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public DateTime FechaEstreno { get; set; }
        public enum CrearTipoClasificacion
        {
            siete,
            doce,
            quince,
            dieciocho
        }
        public CrearTipoClasificacion clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }
    }
}
