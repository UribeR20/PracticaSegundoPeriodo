using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_ReservaController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public estados_ReservaController(EquiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        //Create Method
        [HttpPost]
        [Route("AddStateBook")]
        public IActionResult AddStateBook([FromBody] estados_reserva stateReserva)
        {
            try
            {
                _equiposContext.estados_reserva.Add(stateReserva);
                _equiposContext.SaveChanges();
                return Ok(stateReserva);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        //Read Mehtod
        [HttpGet]
        [Route("GetAllState")]
        public ActionResult Get()
        {
            try
            {
                List<estados_reserva> stateReserva = (from sr in _equiposContext.estados_reserva select sr).ToList();

                if (stateReserva.Count == 0)
                {
                    return NotFound();
                }
                return Ok(stateReserva);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("upadateState/{id}")]
        public IActionResult updateState(int id, [FromBody] estados_equipo stateReservaModificar)
        {
            try
            {
                estados_reserva? stateReserva = (from sr in _equiposContext.estados_reserva where sr.estado_res_id == id select sr).FirstOrDefault();

                if (stateReserva == null) return NotFound();
                
                stateReserva.estado = stateReservaModificar.estado;

                _equiposContext.Entry(stateReservaModificar).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(stateReservaModificar);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Delete
        [HttpDelete]
        [Route("deleteState/{id}")]
        public IActionResult deleteState(int id)
        {
            try
            {
                estados_reserva? stateReserva = (from sr in _equiposContext.estados_reserva where sr.estado_res_id == id select sr).FirstOrDefault();

                if (stateReserva == null) return NotFound();

                _equiposContext.estados_reserva.Attach(stateReserva);
                _equiposContext.estados_reserva.Remove(stateReserva);
                _equiposContext.SaveChanges();
                return Ok(stateReserva);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
