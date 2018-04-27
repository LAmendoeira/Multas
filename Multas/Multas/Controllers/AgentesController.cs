using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {
        private MultasDb db = new MultasDb();

        // GET: Agentes
        public ActionResult Index()
        {
            //SELECT * FROM Agentes ORDER BY Nome
            var listaAgentes = db.Agentes.OrderBy(a => a.Nome).ToList();
            return View(listaAgentes);
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// Apresenta a listagem dos dados de um agente
        /// </summary>
        /// <param name="id">Indentifica o numero do agente a pesquisar</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase carregaFotografia)
        {

            //Gerar ID para o novo agente
            int novoID = 1;
            if (db.Agentes.Count() != 0)
                novoID = db.Agentes.Max(a => a.ID) + 1;

            agente.ID = novoID;//Atribuir novo ID ao agente
            var filename = "Agente_" + novoID + DateTime.Now.ToString("_yyyyMMdd_hhmmss") + ".jpg";
            var imagePath = "";

            //Validar se existe imagem
            if (carregaFotografia != null)//Existe um ficheiro
            {
                agente.Fotografia = filename;
                imagePath = Path.Combine(Server.MapPath("~/imagens/"), filename);
            }
            else
            {
                //Gerar uma mensagem de erro para que o utilizador saiba o que se passou para a inserção não ter corrido bem
                ModelState.AddModelError("", "Não foi inserida uma imagem.");
                //Não foi carregada uma fotografia
                return View(agente);
            }

            //Verificar se o ficheiro carregado é mesmo uma imagem
            if (!ImagemValida(carregaFotografia))
            {
                ModelState.AddModelError("", "O ficheiro fornecido não é uma imagem válida.");
                return View(agente);
            }


            if (ModelState.IsValid)
            {
                db.Agentes.Add(agente);
                try
                {


                    db.SaveChanges();

                    //Guardar imagem no disco

                    carregaFotografia.SaveAs(imagePath);

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Não foi possivel inserir o Agente " + agente.Nome + ", por favor tente mais tarde ou contacte o Administrador.");
                }
            }

            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Fotografia,Esquadra")] Agentes agente, HttpPostedFileBase carregaFotografia)
        {

            string filename = "";
            string imagePath = "";
            string oldName = "";
            //Verificar se existe uma fotografia
            if (carregaFotografia != null)//Existe um ficheiro
            {
                filename = "agente_" + agente.ID + DateTime.Now.ToString("_yyyyMMdd_hhmmss") + ".jpg";
                imagePath = Path.Combine(Server.MapPath("~/imagens/"), filename);
                oldName = agente.Fotografia;
                agente.Fotografia = filename;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(agente).State = EntityState.Modified;
                    db.SaveChanges();
                    if (carregaFotografia != null)//Existe um ficheiro
                    {
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/imagens/"), oldName));
                        carregaFotografia.SaveAs(imagePath);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "erro");
                }

            }
            return View(agente);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agentes agente = db.Agentes.Find(id);
            try
            {
                db.Agentes.Remove(agente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", string.Format("A coisa correu mal na eliminação do Agente '{0}', existem multas associadas a ele", agente.Nome));
            }
            //Se o fluxo passar por aqui é porque alguma coisa correu mal
            return View(agente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Se o ficheiro passado por parametro for uma imagem devolve true
        public bool ImagemValida(HttpPostedFileBase file)
        {
            //verificar a extensão do ficheiro
            string extension = Path.GetExtension(file.FileName);

            if (!extension.Equals(".jpg"))
            {
                return false;
            }

            try
            {
                // Ler bytes do ficheiro fornecido
                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes(file.ContentLength);

                string byte1 = binData[0].ToString();
                string byte2 = binData[1].ToString();

                //Os valores do header devem ser FF D8
                if (byte1 == "255" && byte2 == "216")
                {
                    return true;
                }
                return false;
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
