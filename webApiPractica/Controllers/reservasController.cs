using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public reservasController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        //Create a new mark 
        [HttpPost]
        [Route("AddReserva")]
        public IActionResult AddReserva([FromBody] reservas reserva)
        {
            try
            {
                _equiposContext.reservas.Add(reserva);
                _equiposContext.SaveChanges();
                return Ok(reserva);
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
                var listaReserva = (from r in _equiposContext.reservas
                                     join e in _equiposContext.equiposs on r.equipo_id equals e.id_equipos
                                     join us in _equiposContext.usuarios on r.usuario_id equals us.usuario_id
                                     join er in _equiposContext.estados_reserva on r.reserva_id equals er.estado_res_id
                                     select new
                                     {
                                         r.reserva_id,
                                         r.equipo_id,
                                         e.nombre,
                                         e.descripcion,
                                         e.costo,
                                         r.usuario_id,
                                         userName= us.nombre,                                         
                                         us.documento,
                                         us.carnet,
                                         r.fecha_salida,
                                         r.fecha_retorno,
                                         r.tiempo_reserva,
                                         r.estado_reserva_id,
                                         estadoReserva= er.estado,
                                         r.estado
                                     }).ToList();

                if (listaReserva.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listaReserva);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateData(int id, [FromBody] reservas reservasModificar)
        {
            try
            {
                //Check if in the database exist this ID
                reservas? reserva = (from r in _equiposContext.reservas where r.reserva_id == id select r).FirstOrDefault();


                if (reserva == null) return NotFound();

                //If the ID exist, do the following:
                reserva.fecha_salida = reservasModificar.fecha_salida;
                reserva.hora_salida = reservasModificar.hora_salida;
                reserva.tiempo_reserva= reservasModificar.tiempo_reserva;
                reserva.fecha_retorno = reservasModificar.fecha_retorno;
                reserva.hora_retorno = reservasModificar.hora_retorno;

                _equiposContext.Entry(reserva).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(reserva);
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
                reservas? reservaDelete = (from rd in _equiposContext.reservas where rd.reserva_id == id select rd).FirstOrDefault();


                if (reservaDelete == null) return NotFound();

                _equiposContext.reservas.Attach(reservaDelete);
                _equiposContext.reservas.Remove(reservaDelete);
                _equiposContext.SaveChanges();
                return Ok(reservaDelete);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
