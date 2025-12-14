using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;


namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CreateCategoriaDto>().ReverseMap();
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<Pelicula, CreatePeliculaDto>().ReverseMap();
        }
    }
}
