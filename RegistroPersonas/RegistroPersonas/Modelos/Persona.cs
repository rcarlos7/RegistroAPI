using System.ComponentModel.DataAnnotations;

namespace RegistroPersonas.Modelos
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Direccion { get; set; }
        public string? Edad { get; set; }
    }
}
