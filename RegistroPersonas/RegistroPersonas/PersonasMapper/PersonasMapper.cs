using AutoMapper;
using RegistroPersonas.Modelos;
using RegistroPersonas.Modelos.Dtos;

namespace RegistroPersonas.PersonasMapper
{
    public class PersonasMapper : Profile
    {
        public PersonasMapper()
        {
            CreateMap<Persona, PersonaDto>().ReverseMap();
            CreateMap<Persona, CrearPersonaDto>().ReverseMap();
            CreateMap<Persona, ActualizarPersonaDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();
        }

    }
}
