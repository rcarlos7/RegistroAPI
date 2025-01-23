namespace RegistroPersonas.Modelos.Dtos
{
    public class ActualizarPersonaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string? Edad { get; set; }
    }
}
