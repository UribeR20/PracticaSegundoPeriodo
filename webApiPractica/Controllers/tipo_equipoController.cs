using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public tipo_equipoController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        //Create a new mark 
        [HttpPost]
        [Route("AddTypeEquip")]
        public IActionResult addMark([FromBody] tipo_equipo newTipo_Equipo)
        {
            try
            {
                _equiposContext.Add(newTipo_Equipo);
                _equiposContext.SaveChanges();
                return Ok(newTipo_Equipo);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        // Read Method
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            try
            {
                List<tipo_equipo> listTypeEquip = (from tp in _equiposContext.tipo_equipo select tp).ToList();

                if (listTypeEquip.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listTypeEquip);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateData(int id, [FromBody] tipo_equipo tipo_EquipoModificar)
        {
            try
            {
                //Check if in the database exist this ID
                tipo_equipo? tipoEquipo = (from m in _equiposContext.tipo_equipo where m.id_tipo_equipo == id select m).FirstOrDefault();


                if (tipoEquipo == null) return NotFound();

                //If the ID exist, do the following:
                tipoEquipo.descripcion = tipo_EquipoModificar.descripcion;
                tipoEquipo.estado = tipo_EquipoModificar.estado;

                _equiposContext.Entry(tipoEquipo).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(tipoEquipo);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                //Check if in the database exist this ID
                tipo_equipo? tipoEquipo = (from m in _equiposContext.tipo_equipo where m.id_tipo_equipo == id select m).FirstOrDefault();


                if (tipoEquipo == null) return NotFound();

                _equiposContext.tipo_equipo.Attach(tipoEquipo);
                _equiposContext.tipo_equipo.Remove(tipoEquipo);
                _equiposContext.SaveChanges();
                return Ok(tipoEquipo);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
