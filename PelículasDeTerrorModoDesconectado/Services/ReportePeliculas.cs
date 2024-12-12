using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PelículasDeTerrorModoDesconectado.Models;

namespace PelículasDeTerrorModoDesconectado.Services
{
    public class ReportePeliculas
    {
        public byte[] GetReportePeliculas(List<Pelicula> peliculas)
        {
            MemoryStream archivoMemoria = new();
            Document documentoPDF = new(PageSize.LETTER);
            PdfWriter.GetInstance(documentoPDF, archivoMemoria);
            documentoPDF.Open();
            Font fuenteTitulo = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.RED);
            Font fuenteDato = FontFactory.GetFont("Arial", 12, Font.ITALIC, BaseColor.BLUE);
            documentoPDF.Add(new Paragraph("Reporte de películas en biblioteca")
            {
                Alignment = Element.ALIGN_CENTER,
                Font = fuenteTitulo,
            });
            documentoPDF.Add(new Paragraph("\n"));
            PdfPTable tabla = new(6);
            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] { .5f, 3.5f, 1.2f, 1.6f, .85f, 3f });
            PdfPCell[] encabezados = new PdfPCell[] { new(new Phrase("No.", fuenteDato)), new(new Phrase("Nombre", fuenteDato)), new(new Phrase("Duración", fuenteDato)), new(new Phrase("Estreno", fuenteDato)), new(new Phrase("Rating", fuenteDato)), new(new Phrase("Director", fuenteDato))};
            for(int i = 0; i < encabezados.Length; i++)
            {
                encabezados[i].BackgroundColor = BaseColor.LIGHT_GRAY;
                encabezados[i].HorizontalAlignment = Element.ALIGN_CENTER;
                tabla.AddCell(encabezados[i]);
            }
            int totalRenglones = 1;
            foreach (var p in peliculas)
            {
                tabla.AddCell(new PdfPCell(new Phrase(totalRenglones.ToString()))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                tabla.AddCell(new PdfPCell(new Phrase(p.Nombre)));
                tabla.AddCell(new PdfPCell(new Phrase(p.Duracion.ToString("0 min"))));
                tabla.AddCell(new PdfPCell(new Phrase(p.Estreno.ToString("dd/MM/yyyy"))));
                tabla.AddCell(new PdfPCell(new Phrase(p.Rating.ToString("0/5"))));
                tabla.AddCell(new PdfPCell(new Phrase(p.Director)));
                totalRenglones++;
            }
            documentoPDF.Add(tabla);
            documentoPDF.Close();
            return archivoMemoria.ToArray();
        }
    }
}
