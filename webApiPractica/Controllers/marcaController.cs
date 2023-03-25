using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcaController : ControllerBase
    {
        
        private readonly EquiposContext _equiposContext;
        public marcaController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }
        
        
        [HttpPost]
        [Route("AddMark")]
        public IActionResult addMark([FromBody] marcas marcas)
        {
            try
            {
                _equiposContext.marcas.Add(marcas);
                _equiposContext.SaveChanges();
                return Ok(marcas);
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
                List<marcas> listMark = (from lm in _equiposContext.marcas select lm).ToList();

                if (listMark.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listMark);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);             }
            }

        
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateData(int id, [FromBody] marcas marcasModificar)
        {
            try
            {
                
                marcas? marcas = (from m in _equiposContext.marcas where m.id_marcas == id select m).FirstOrDefault();


                if (marcas == null) return NotFound();

                
                marcas.nombre_marca = marcasModificar.nombre_marca;
                marcas.estados = marcasModificar.estados;

                _equiposContext.Entry(marcas).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(marcas);
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
                
                marcas? marcas = (from m in _equiposContext.marcas where m.id_marcas == id select m).FirstOrDefault();


                if (marcas == null) return NotFound();

                _equiposContext.marcas.Attach(marcas);
                _equiposContext.marcas.Remove(marcas);
                _equiposContext.SaveChanges();
                return Ok(marcas);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
