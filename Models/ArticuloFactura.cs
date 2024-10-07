using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_TiendaElectronica.Models
{
    public class ArticuloFactura
    {
        public int ArticuloFacturaId { get; set; }

        public int idArticulo { get; set; }

        public int idFactura { get; set; }

        public int CantidadArticulo { get; set; }

        [ForeignKey("idArticulo")]
        public Articulo Articulo { get; set; }

        [ForeignKey("idFactura")]
        public Factura Factura { get; set; }
    }
}
