using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.ItRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly AppDbContext _db;

        public PeliculaRepository(AppDbContext db)
        {
            _db = db;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            var peliculaExistente = _db.Peliculas.Find(pelicula.Id);
            if (peliculaExistente != null)
            {
                _db.Entry(peliculaExistente).CurrentValues.SetValues(pelicula);
            }
            else
            {
                _db.Peliculas.Update(pelicula);
            }
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _db.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _db.Peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre));
            }
            return query.ToList();

        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _db.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(int peliculaId)
        {
             return _db.Peliculas.Any(p => p.Id == peliculaId);
        }

        public bool ExistePelicula(string nombre)
        {
            return _db.Peliculas.Any(p => p.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _db.Peliculas.FirstOrDefault(p => p.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _db.Peliculas.OrderBy(p => p.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _db.Peliculas.Include(c => c.Categoria).Where(c => c.CategoriaId == categoriaId).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
