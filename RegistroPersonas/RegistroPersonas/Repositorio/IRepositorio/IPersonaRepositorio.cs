using RegistroPersonas.Modelos;

namespace RegistroPersonas.Repositorio.IRepositorio
{
    public interface IPersonaRepositorio
    {
        ICollection<Persona> GetPersonas();
        Persona GetPersona(int PersonaId);
        IEnumerable<Persona> BuscarPersona(string nombre);
        bool ExistePersona(int id);
        bool ExistePersona(string nombre, string apellido);
        bool CrearPersona(Persona persona);
        bool ActualizarPersona(Persona persona);
        bool BorrarPersona(Persona persona);
        bool Guardar();
    }
}
