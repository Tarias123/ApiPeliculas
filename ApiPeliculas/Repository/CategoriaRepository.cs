using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.ItRepository;

namespace ApiPeliculas.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _db;
        public CategoriaRepository(AppDbContext db)
        {
            _db = db;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            var categoriaDb = _db.Categorias.Find(categoria.Id);
            if(categoriaDb != null)
            {
                _db.Categorias.Entry(categoriaDb).CurrentValues.SetValues(categoria);
            }
            return Guardar();
        }

        public bool CategoriaExiste(string nombre)
        {
            bool valor = _db.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool CategoriaExisteId(int categoriaId)
        {
            bool valor = _db.Categorias.Any(c => c.Id == categoriaId);
            return valor;
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categorias.Add(categoria);
            return Guardar();
        }

        public bool DeleteCategoria(int categoriaId)
        {
            _db.Categorias.Remove(_db.Categorias.Find(categoriaId));
            return Guardar();
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _db.Categorias.FirstOrDefault(c => c.Id == categoriaId);

        }

        public Categoria GetCategoriaPorNombre(string nombre)
        {
            return _db.Categorias.FirstOrDefault(c => c.Nombre == nombre);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _db.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
