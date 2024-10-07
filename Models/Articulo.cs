using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Proyecto_TiendaElectronica.Models
{
    public class Articulo
    {
        [Key]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "El nombre del artículo es obligatorio")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre del artículo debe tener entre 1 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La descripción debe tener entre 1 y 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "La marca debe tener entre 1 y 50 caracteres")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        [ForeignKey("Imagen")]
        public int codigoImagen { get; set; }

        [ForeignKey("Categoria")]
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int idCategoria { get; set; }

        public Imagen Imagen { get; set; }
        public Categoria Categoria { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Debe seleccionar una imagen 1")]
        public IFormFile Imagen1 { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Debe seleccionar una imagen 2")]
        public IFormFile Imagen2 { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Debe seleccionar una imagen 3")]
        public IFormFile Imagen3 { get; set; }
    }
}
