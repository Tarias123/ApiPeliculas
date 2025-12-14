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
            //Arreglar problemas con el put 
            var categoriaDb = _db.Categorias.FirstOrDefault(c => c.Id == categoria.Id);
            if (categoriaDb != null)
            {
                _db.Entry(categoriaDb).CurrentValues.SetValues(categoria);
            }
            else
            {
                _db.Categorias.Update(categoria);
            }
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _db.Categorias.Remove(categoria);
            return Guardar();
        }
        
        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(int categoriaId)
        {
            return _db.Categorias.Any(c => c.Id == categoriaId);
        }

        public bool ExisteCategoria(string nombre)
        {
           bool valor = _db.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
           return valor;
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _db.Categorias.FirstOrDefault(c => c.Id == categoriaId);
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
