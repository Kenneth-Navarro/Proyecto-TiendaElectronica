using System.ComponentModel.DataAnnotations;
using Proyecto_TiendaElectronica.ViewModels;

namespace Proyecto_TiendaElectronica.Models
{
    public class Factura
    {
        [Key]
        public int FacturaId { get; set; }
        public DateTime FechaCreacion { get; set; }

        public DateTime UltimaFechaImpresion { get; set; }

        public decimal SubTotal { get; set; }

        public decimal MontoTotal { get; set; }

        public string UsuarioId { get; set; }

        public List<ArticuloFactura> articulosFactura {  get; set; }                             
    }                                             
}                                                 
                                                  