using PelículasDeTerrorModoDesconectado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelículasDeTerrorModoDesconectado.Repositories
{
    public class PeliculasRepository
    {
        PelículasDeTerrorContext context = new();

        public IEnumerable<Pelicula> GetAll()
        {
            return context.Pelicula.OrderBy(x => x.Nombre);
        }

        public IEnumerable<string> GetAllDirectores()
        {
            return context.Pelicula.Select(x=>x.Director).Distinct();
        }

        public int CountPelículas()
        {
            return context.Pelicula.Count();
        }

        public IEnumerable<Pelicula> GetPeliculasRating(int r)
        {
            return context.Pelicula.Where(x => x.Rating == r);
        }

        public int CountRating(int r)
        {
            return context.Pelicula.Count(x => x.Rating == r);
        }

        public IEnumerable<Pelicula> GetPelículasAñoEstreno(int e)
        {
            return context.Pelicula.Where(x => x.Estreno.Year == e);
        }

        public int CountPeliculasAñoEstreno(int e)
        {
            return context.Pelicula.Count(x=>x.Estreno.Year == e);
        }

        public void Agregar(Pelicula p)
        {
            context.Pelicula.Add(p);
            context.SaveChanges();
        }

        public void Eliminar(Pelicula p)
        {
            context.Pelicula.Remove(p);
            context.SaveChanges();
        }

        public Pelicula? GetById(int id)
        {
            return context.Pelicula.Find(id);
        }

        public void Editar(Pelicula p)
        {
            context.Pelicula.Update(p);
            context.SaveChanges();
        }

        public static bool Validar(Pelicula p, out string? error)
        {
            List<string> listErrores = new();
            if (string.IsNullOrWhiteSpace(p.Nombre) || string.IsNullOrWhiteSpace(p.Director) || string.IsNullOrWhiteSpace(p.Sinopsis) || string.IsNullOrWhiteSpace(p.Portada))
                listErrores.Add("Verifica haber llenado todos los campos");
            if (p.Duracion < 30)
                listErrores.Add("Verifica que la duración sea correcta");
            if (p.Estreno > DateTime.Now)
                listErrores.Add("Verifica que la fecha esté en formato YYYY-MM-DD y que sea correcta");
            if (p.Rating < 1 || p.Rating > 5)
                listErrores.Add("El Rating es evaluado en una escala de 1 a 5");
            if(p.Portada is not null)
                if (!p.Portada.EndsWith(".jpg") && !p.Portada.EndsWith(".jpeg") && !p.Portada.EndsWith(".png"))
                    listErrores.Add("Verifica que el formato de la imagen termine en jpg, jpeg o png");

            error = string.Join(Environment.NewLine, listErrores);
            return listErrores.Count != 0;
        }
    }
}
