using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaJavierManriquez.Models
{
    public class ProductoDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 99999999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La existencia es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La existencia no puede ser negativa.")]
        public int Existencia { get; set; }
    }
}
