using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly  EquiposContext _equiposContext;
        public equiposController(EquiposContext equiposContexto) 
        {
            _equiposContext = equiposContexto;
        
        }
        
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
           
            var listadoequipo = (from e in _equiposContext.equiposs 
                                           join m in _equiposContext.marcas on e.marca_id equals m.id_marcas
                                           join te in _equiposContext.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                 select new
                                           {                                               
                                               e.id_equipos, 
                                               e.nombre, 
                                               e.descripcion, 
                                               e.tipo_equipo_id, 
                                               tipo_equipo= te.descripcion, 
                                               e.marca_id, 
                                               m.nombre_marca
                                           }).ToList();
            
            if (listadoequipo.Count()==0)
            {
                return NotFound();
            }
                        
            return Ok(listadoequipo);
        }
       
        
        [HttpGet]
        [Route("getbyid/{id}")]

       

        public IActionResult GetById(int id)
        {
            
            equiposs? equipo = (from e in _equiposContext
                          .equiposs where e.id_equipos== id
                          select e).FirstOrDefault();
            
            
            if(equipo == null) return NotFound();
            return Ok(equipo);          
     
        }

        [HttpGet]
        [Route("find/{filtro}")]
        public IActionResult GetByName(string filtro)
        {
     
            List<equiposs> equipo = (from e in _equiposContext
                          .equiposs
                               where e.nombre.Contains(filtro)
                               select e).ToList();
            

            if (equipo == null) return NotFound();
            return Ok(equipo);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody]equiposs equipo)
        {
            try
            {
                _equiposContext.equiposs.Add(equipo);
                _equiposContext.SaveChanges();
                return Ok(equipo);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult Actualizar(int id, [FromBody] equiposs equipoActualizar)
        {
        
            equiposs? equipo = (from e in _equiposContext
                         .equiposs
                               where e.id_equipos == id
                               select e).FirstOrDefault();            

            if (equipo == null) return NotFound();
           

            equipo.nombre = equipoActualizar.nombre;
            equipo.descripcion = equipoActualizar.descripcion;
            equipo.modelo = equipoActualizar.modelo;
            equipo.costo = equipoActualizar.costo;
            equipo.estado = equipoActualizar.estado;

            _equiposContext.Entry(equipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(equipo);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult Delete(int id)
        {
            equiposs? equipo = (from e in _equiposContext
                         .equiposs
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null) return NotFound();

            _equiposContext.equiposs.Attach(equipo);
            _equiposContext.equiposs.Remove(equipo);
            _equiposContext.SaveChanges();
            return Ok(equipo);
        }
        
    }
}
