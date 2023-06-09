﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        
        private readonly EquiposContext _equiposContext;

        public usuariosController(EquiposContext equiposContexto)
        {
            _equiposContext= equiposContexto;
        }

       
        [HttpPost]
        [Route("AddUser")]
        public IActionResult addUser([FromBody] usuarios user)
        {
            try
            {
                _equiposContext.usuarios.Add(user);
                _equiposContext.SaveChanges();
                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet]
        [Route("GetAll")]
        public IActionResult get()
        {
            try
            {
                var userList = (from u in _equiposContext.usuarios
                                     join c in _equiposContext.carreras on u.carrera_id equals c.carrera_id
                                     
                                     select new
                                     {
                                         u.usuario_id,
                                         u.nombre,
                                         u.documento,
                                         u.tipo,
                                         u.carnet,
                                         u.carrera_id,
                                         c.nombre_carrera,
                                         u.estado
                                     }).ToList();

               
                if (userList.Count==0)
                {
                    return NotFound();
                }
                return Ok(userList);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("userUpdate/{id}")]
        public IActionResult updateData(int id, [FromBody] usuarios usuarioModificar)
        {
            try
            {
                
                usuarios? userUpdate = (from u in _equiposContext.usuarios where u.usuario_id == id select u).FirstOrDefault();


                if (userUpdate == null) return NotFound();

                
                userUpdate.nombre = usuarioModificar.nombre;
                userUpdate.documento = usuarioModificar.documento;
                userUpdate.tipo = usuarioModificar.tipo;
                userUpdate.carnet = usuarioModificar.carnet;

                _equiposContext.Entry(userUpdate).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(userUpdate);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        [Route("userDelete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                
                usuarios? userDelete = (from u in _equiposContext.usuarios where u.usuario_id == id select u).FirstOrDefault();


                if (userDelete == null) return NotFound();

                _equiposContext.usuarios.Attach(userDelete);
                _equiposContext.usuarios.Remove(userDelete);
                _equiposContext.SaveChanges();
                return Ok(userDelete);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}
