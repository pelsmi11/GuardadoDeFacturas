using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using prueva10.Models;

namespace prueva10.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FacturasController : ApiController
    {
        private DB_LazaroEntities db = new DB_LazaroEntities();

        // GET: api/Facturas
        public IQueryable<Factura> GetFactura()
        {
            return db.Factura;
        }

        // GET: api/Facturas/5
        [ResponseType(typeof(Factura))]
        public IHttpActionResult GetFactura(int id)
        {
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return NotFound();
            }

            return Ok(factura);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/blop")]
        public async Task<string> Post()
        {
            var httpContext = HttpContext.Current;
            try
            {
                // Check for any uploaded file  
                if (httpContext.Request.Files.Count > 0)
                {
                    //Loop through uploaded files  
                    for (int i = 0; i < httpContext.Request.Files.Count; i++)
                    {
                        HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                        if (httpPostedFile != null)
                        {
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference("blobimages");
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(httpPostedFile.FileName);
                            blockBlob.UploadFromStream(httpPostedFile.InputStream);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                return "error: " + e;
            }
            return "File uploaded!";
        }

      

        // PUT: api/Facturas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFactura(int id, Factura factura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != factura.Id_Factura)
            {
                return BadRequest();
            }

            db.Entry(factura).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
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
        
        // POST: api/Facturas
        [ResponseType(typeof(Factura))]
        public IHttpActionResult PostFactura(Factura factura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Factura.Add(factura);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = factura.Id_Factura }, factura);
        }
        // POST: api/Facturas
        /*[ResponseType(typeof(Factura))]
        public IHttpActionResult PostFactura(Factura factura)
        {
            string nombre="";
            var httpContext = HttpContext.Current;
            try
            {
                // Check for any uploaded file  
                if (httpContext.Request.Files.Count > 0)
                {
                    //Loop through uploaded files  
                    for (int i = 0; i < httpContext.Request.Files.Count; i++)
                    {
                        HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                        if (httpPostedFile != null)
                        {
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference("blobimages");
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(httpPostedFile.FileName);
                            nombre = httpPostedFile.FileName;
                            blockBlob.UploadFromStream(httpPostedFile.InputStream);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            factura.Factura_ruta = nombre;

            db.Factura.Add(factura);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = factura.Id_Factura }, factura);
        }*/

        // DELETE: api/Facturas/5
        [ResponseType(typeof(Factura))]
        public IHttpActionResult DeleteFactura(int id)
        {
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return NotFound();
            }

            db.Factura.Remove(factura);
            db.SaveChanges();

            return Ok(factura);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FacturaExists(int id)
        {
            return db.Factura.Count(e => e.Id_Factura == id) > 0;
        }
    }
}