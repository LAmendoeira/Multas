using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Multas.Models;

namespace Multas.Api
{
    public class ViaturasController : ApiController
    {
        private MultasDb db = new MultasDb();

        // GET: api/Viaturas
        public IHttpActionResult GetViaturas()
        {
            var result = db.Viaturas
                .Select(viatura => new
                {
                    viatura.ID,
                    viatura.Marca,
                    viatura.Modelo,
                    viatura.Matricula,
                    viatura.MoradaDono,
                    viatura.NomeDono
                })
                .ToList();
            return Ok(result);
        }

        // GET: api/Viaturas/5
        [ResponseType(typeof(Viaturas))]
        public IHttpActionResult GetViaturas(int id)
        {
            Viaturas viaturas = db.Viaturas.Find(id);
            if (viaturas == null)
            {
                return NotFound();
            }

            return Ok(viaturas);
        }

        // PUT: api/Viaturas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutViaturas(int id, Viaturas viaturas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != viaturas.ID)
            {
                return BadRequest();
            }

            db.Entry(viaturas).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViaturasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Viaturas
        [ResponseType(typeof(Viaturas))]
        public IHttpActionResult PostViaturas(Viaturas viaturas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Viaturas.Add(viaturas);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = viaturas.ID }, viaturas);
        }

        // DELETE: api/Viaturas/5
        [ResponseType(typeof(Viaturas))]
        public IHttpActionResult DeleteViaturas(int id)
        {
            Viaturas viaturas = db.Viaturas.Find(id);
            if (viaturas == null)
            {
                return NotFound();
            }

            db.Viaturas.Remove(viaturas);
            db.SaveChanges();

            return Ok(viaturas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ViaturasExists(int id)
        {
            return db.Viaturas.Count(e => e.ID == id) > 0;
        }
    }
}