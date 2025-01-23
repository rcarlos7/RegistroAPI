using RegistroPersonas.Data;
using RegistroPersonas.Modelos;
using RegistroPersonas.Repositorio.IRepositorio;

namespace RegistroPersonas.Repositorio
{
    public class PersonaRepositorio : IPersonaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public PersonaRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarPersona(Persona persona)
        {
            var personaExistente = _db.Persona.Find(persona.Id);
            if (personaExistente != null)
            {
                _db.Entry(personaExistente).CurrentValues.SetValues(persona);
            }
            else
            {
                _db.Persona.Update(persona);
            }
            return Guardar();
        }

        public bool BorrarPersona(Persona persona)
        {
            _db.Persona.Remove(persona);
            return Guardar();
        }

        public IEnumerable<Persona> BuscarPersona(string nombre)
        {
            IQueryable<Persona> query = _db.Persona;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Apellido.Contains(nombre));
            }
            return query.ToList();
        }

        public bool CrearPersona(Persona persona)
        {
            _db.Persona.Add(persona);
            return Guardar();
        }

        public bool ExistePersona(int id)
        {
            return _db.Persona.Any(p => p.Id == id);
        }

        public bool ExistePersona(string nombre, string apellido)
        {
            bool valor = _db.Persona.Any(p => p.Nombre.ToLower().Trim() == nombre.ToLower().Trim()) && _db.Persona.Any(p => p.Apellido.ToLower().Trim() == apellido.ToLower().Trim());
            return valor;
        }

        public Persona GetPersona(int PersonaId)
        {
            return _db.Persona.FirstOrDefault(p =>  p.Id == PersonaId);
        }

        public ICollection<Persona> GetPersonas()
        {
            return _db.Persona.OrderBy(p => p.Id).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
