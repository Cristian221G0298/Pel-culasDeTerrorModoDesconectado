using GalaSoft.MvvmLight.Command;
using PelículasDeTerrorModoDesconectado.Models;
using PelículasDeTerrorModoDesconectado.Repositories;
using PelículasDeTerrorModoDesconectado.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PelículasDeTerrorModoDesconectado.ViewModels
{
    public enum Vistas { Principal, Agregar, Eliminar, Detalles, Editar }
    public class PeliculasDeTerrorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        PeliculasRepository repos = new();
        public Pelicula? Pelicula { get; set; }
        public Vistas Vista { get; set; }
        public List<Pelicula> Películas { get; set; } = new();
        public List<string> Filtros { get; set; } = ["Todas", "Año", "Rating"];
        string? seleccion;
        public string? Seleccion
        {
            get { return seleccion; }
            set { seleccion = value; Actualizar(nameof(Seleccion)); if (value == "Todas") Actualizar(); }
        }
        int año;
        public int Año
        {
            get { return año; }
            set { año = value; FiltrarEstreno(value); }
        }
        int rating;
        public int Rating
        {
            get { return rating; }
            set { rating = value; FiltrarRating(value); }
        }
        public IEnumerable<string> Directores => repos.GetAllDirectores().ToList();
            ///Películas.Select(x => x.Director).Distinct();
        public IEnumerable<int> Años => repos.GetAll().OrderBy(x => x.Estreno).Select(x => x.Estreno.Year).Distinct();
        public IEnumerable<int> Ratings { get; set; } = [1, 2, 3, 4, 5];
        public int Cantidad { get; set; }
        string? error;
        public string? Error { get => error; set => error = value; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand {  get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand FiltrarEstrenoCommand { get; set; }
        public ICommand FiltrarRatingCommand { get; set; }
        public ICommand ImprimirCommand {  get; set; }

        public PeliculasDeTerrorViewModel()
        {
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            EditarCommand = new RelayCommand(Editar);
            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            FiltrarEstrenoCommand = new RelayCommand<int>(FiltrarEstreno);
            FiltrarRatingCommand = new RelayCommand<int>(FiltrarRating);
            ImprimirCommand = new RelayCommand(Imprimir);
            
            Actualizar();
        }
        void Actualizar()
        {
            Películas.Clear();
            Películas = repos.GetAll().ToList();
            Actualizar(nameof(Películas));
            Cantidad = repos.CountPelículas();
            Actualizar(nameof(Cantidad));
        }
        void Actualizar(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
        void CambiarVista(Vistas v)
        {
            if (v == Vistas.Agregar)
                Pelicula = new();

            Vista = v;
            Actualizar(nameof(Vista));
            Actualizar(nameof(Pelicula));
        }
        void Agregar()
        {
            if (Pelicula is not null)
                if (!PeliculasRepository.Validar(Pelicula, out error))
                {
                    repos.Agregar(Pelicula);
                    Actualizar(nameof(Cantidad));
                    CambiarVista(Vistas.Principal);
                    Actualizar(nameof(Vista));
                    Actualizar();
                }
            Actualizar(nameof(Error));
        }
        void Eliminar()
        {
            if (Pelicula is not null)
            {
                repos.Eliminar(Pelicula);
                CambiarVista(Vistas.Principal);
                Actualizar(nameof(Vista));
                Actualizar();
            }
        }
        void Editar()
        {
            if (Pelicula is not null)
            {
                if (!PeliculasRepository.Validar(Pelicula, out error))
                {
                    Pelicula? clon = repos.GetById(Pelicula.Id);
                    if (clon is not null)
                    {
                        clon.Nombre = Pelicula.Nombre;
                        clon.Rating = Pelicula.Rating;
                        clon.Sinopsis = Pelicula.Sinopsis;
                        clon.Director = Pelicula.Director;
                        clon.Duracion = Pelicula.Duracion;
                        clon.Estreno = Pelicula.Estreno;
                        repos.Editar(clon);
                        CambiarVista(Vistas.Principal);
                        Actualizar();
                    }
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        void FiltrarEstreno(int x)
        {
            Películas.Clear();
            Películas = repos.GetPelículasAñoEstreno(x).ToList();
            Actualizar(nameof(Películas));
            Cantidad = repos.CountPeliculasAñoEstreno(x);
            Actualizar(nameof(Cantidad));
        }
        void FiltrarRating(int x)
        {
            Películas.Clear();
            Películas = repos.GetPeliculasRating(x).ToList();
            Actualizar(nameof(Películas));
            Cantidad = repos.CountRating(x);
            Actualizar(nameof(Cantidad));
        }
        void Imprimir()
        {
            ReportePeliculas reporte = new();
            byte[] pdfBytes = reporte.GetReportePeliculas(Películas);
            File.WriteAllBytes("Reporte.pdf", pdfBytes);
        }
    }
}