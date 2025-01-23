using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistroPersonas.Modelos;
using RegistroPersonas.Modelos.Dtos;
using RegistroPersonas.Repositorio.IRepositorio;
using System.Net;

namespace RegistroPersonas.Controllers.v1
{
    [Route("api/v{version:apiVersion}/personas")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaRepositorio _perRepo;
        protected RespuestaApi _respuestaApi;
        private readonly IMapper _mapper;

        public PersonasController(IPersonaRepositorio pelRepo, IMapper mapper)
        {
            _perRepo = pelRepo;
            _respuestaApi = new();
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetPersona()
        {
            var listaPersonas = _perRepo.GetPersonas();
            var ListaPersonasDto = new List<PersonaDto>();

            foreach (var lista in listaPersonas)
            {
                ListaPersonasDto.Add(_mapper.Map<PersonaDto>(lista));
            }
            return Ok(ListaPersonasDto);
        }

        [HttpGet("{personaId:int}", Name = "GetPersona")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetPersona(int personaId)
        {
            var itemPersona = _perRepo.GetPersona(personaId);

            if (itemPersona == null)
            {
                return NotFound();
            }

            var itemPersonaDto = _mapper.Map<PersonaDto>(itemPersona);

            return Ok(itemPersonaDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PersonaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult crearPersona([FromForm] CrearPersonaDto crearPersonaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearPersonaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_perRepo.ExistePersona(crearPersonaDto.Nombre, crearPersonaDto.Apellido))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre y apellido ya existe");
                return BadRequest(_respuestaApi);
            }

            var persona = _mapper.Map<Persona>(crearPersonaDto);

            if (!_perRepo.CrearPersona(persona))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error en el registro");
                return BadRequest(_respuestaApi);
            }

            return CreatedAtRoute("GetPersona", new { personaId = persona.Id }, persona);
        }

        [HttpPatch("{personaId:int}", Name = "ActualizarPerosna")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult ActualizarPersona(int personaId, [FromForm] ActualizarPersonaDto actualizarPersonaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actualizarPersonaDto == null || personaId != actualizarPersonaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var personaExistente = _perRepo.GetPersona(personaId);
            if (personaExistente == null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add($"La persona con el id {personaExistente} no fue encontrada");
                return BadRequest(_respuestaApi);
            }

            var persona = _mapper.Map<Persona>(actualizarPersonaDto);

            _perRepo.ActualizarPersona(persona);
            return NoContent();
        }

        [HttpDelete("{personaId:int}", Name = "BorrarPersona")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult BorrarPersona(int personaId)
        {
            if (!_perRepo.ExistePersona(personaId))
            {
                return NotFound();
            }

            var persona = _perRepo.GetPersona(personaId);

            if (!_perRepo.BorrarPersona(persona))
            {
                _respuestaApi.StatusCode=HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess=false;
                _respuestaApi.ErrorMessages.Add("Ocurrio un error al no encontrar el id");
                return BadRequest(_respuestaApi);
            }

            return NoContent();
        }

        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult Buscar(string nombre)
        {
            try
            {
                var persona = _perRepo.BuscarPersona(nombre);
                if (!persona.Any())
                {
                    _respuestaApi.StatusCode =HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.ErrorMessages.Add($"No se encontro la persona con nombre {nombre}");
                    return BadRequest(_respuestaApi);
                }

                var personaDto = _mapper.Map<IEnumerable<PersonaDto>>(persona);
                return Ok(personaDto);
            }
            catch (Exception)
            {
                _respuestaApi.StatusCode = HttpStatusCode.InternalServerError;
                _respuestaApi.IsSuccess = false ;
                _respuestaApi.ErrorMessages.Add("Error recuperando datos de la aplicacion");
                return BadRequest(_respuestaApi);
            }
        }
    }
}
