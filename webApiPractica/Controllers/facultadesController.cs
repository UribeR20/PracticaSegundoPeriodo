using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
       
        private readonly EquiposContext _equiposContext;
        public facultadesController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        
        [HttpPost]
        [Route("AgregarFacultad")]
        public IActionResult addFacultad([FromBody] facultades facultad)
        {
            try
            {
                _equiposContext.facultades.Add(facultad);
                _equiposContext.SaveChanges();
                return Ok(facultad);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

       
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            try
            {
                List<facultades> listaFacultad = (from f in _equiposContext.facultades select f).ToList();

                if (listaFacultad.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listaFacultad);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

      
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateFacultad(int id, [FromBody] facultades facultadModificar)
        {
            try
            {
                
                facultades? facultad = (from f in _equiposContext.facultades where f.facultad_id == id select f).FirstOrDefault();


                if (facultad == null) return NotFound();

               
                facultad.nombre_facultad = facultadModificar.nombre_facultad;
                

                _equiposContext.Entry(facultad).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(facultad);
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
              
                facultades? facultad = (from f in _equiposContext.facultades where f.facultad_id == id select f).FirstOrDefault();


                if (facultad == null) return NotFound();

                _equiposContext.facultades.Attach(facultad);
                _equiposContext.facultades.Remove(facultad);
                _equiposContext.SaveChanges();
                return Ok(facultad);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
