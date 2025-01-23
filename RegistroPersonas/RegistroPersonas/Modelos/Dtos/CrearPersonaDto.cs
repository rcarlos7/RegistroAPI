using System.ComponentModel.DataAnnotations;

namespace RegistroPersonas.Modelos.Dtos
{
    public class CrearPersonaDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string? Edad { get; set; }
    }
}
