using System.ComponentModel.DataAnnotations;

namespace RegistroPersonas.Modelos.Dtos
{
    public class PersonaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre solo debe contener maximo 100 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "La direccion es obligatoria")]
        public string Direccion { get; set; }
        public string? Edad { get; set; }
    }
}
