using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        // Método para obtener todas las villas
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            // Devuelve todas las villas almacenadas en VillaStore.villaList
            return Ok(VillaStore.villaList);
        }

        // Método para obtener una villa por su ID
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            // Validación: Si el ID es 0, retorna un BadRequest
            if (id == 0)
            {
                return BadRequest();
            }

            // Busca la villa por ID en VillaStore.villaList
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            // Si no se encuentra la villa, retorna un NotFound
            if (villa == null)
            {
                return NotFound();
            }

            // Retorna la villa encontrada
            return Ok(villa);
        }

        // Método para crear una nueva villa
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto)
        {
            // Validación del modelo con ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validación personalizada: Verifica si el nombre de la villa ya existe
            if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "El nombre de la villa ya existe");
                return BadRequest(ModelState);
            }

            // Validación adicional: Si el objeto villaDto es nulo, retorna un BadRequest
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }

            // Validación adicional: Si el ID de la villa es mayor que 0, retorna un InternalServerError
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Asigna un nuevo ID a la villa
            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

            // Agrega la nueva villa a VillaStore.villaList
            VillaStore.villaList.Add(villaDto);

            // Retorna un Created con la ruta de la nueva villa
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }

        //Para poder borrar con la id

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();

            }
            VillaStore.villaList.Remove(villa);

            return NoContent();

        }

        //Actualizar los registros

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            villa.Name = villaDto.Name;
            villa.Ocupantes = villaDto.Ocupantes;
            villa.MetrosCuadrados = villaDto.MetrosCuadrados;
            
            return NoContent();
        }


    }
}
