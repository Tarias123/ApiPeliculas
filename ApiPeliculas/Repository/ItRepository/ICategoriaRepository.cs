using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repository.ItRepository
{
    public interface ICategoriaRepository
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int categoriaId);
        Categoria GetCategoriaPorNombre(string nombre);
        bool CategoriaExiste(string nombre);
        bool CategoriaExisteId(int categoriaId);
        bool CrearCategoria(Categoria categoria);
        bool DeleteCategoria(int categoriaId);
        bool ActualizarCategoria(Categoria categoria);
        bool Guardar();
    }
}
