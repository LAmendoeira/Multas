﻿using System;
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
            var filename = "Agente_" + novoID + ".jpg";
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

            //Validar que é uma imagem

            //Escolher nome para imagem

            //Formatar tamanho da imagem

            //Guardar imagem



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
        public ActionResult Edit([Bind(Include = "ID,Nome,Fotografia,Esquadra")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
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
    }
}
