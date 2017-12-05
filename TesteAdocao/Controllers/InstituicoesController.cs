﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TesteAdocao.Models;

namespace TesteAdocao.Controllers
{
    public class InstituicoesController : Controller
    {
        private static Instituicao idsessao;
        private TesteAdocaoContext db = new TesteAdocaoContext();

        // GET: Instituicoes
        public ActionResult Index()
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                int id = Convert.ToInt32(Session["LoginAdministrador"]);
                return View(db.Instituicaos.Where(x => x.InstituicaoId == id).ToList());
            }
        }

        // GET: Instituicoes/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Instituicao instituicao = db.Instituicaos.Find(id);
                if (instituicao == null)
                {
                    return HttpNotFound();
                }
                return View(instituicao);
            }
        }

        // GET: Instituicoes/Create
        public ActionResult Create()
        {
            
            
                return View();
            
        }

        // POST: Instituicoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InstituicaoId,InstituicaoLogin,InstituicaoSenha,Status,InstituicaoNome,InstituicaoTelefone,InstituicaoEmail,InstituicaoEndereco")] Instituicao instituicao)
        { 
                if (ModelState.IsValid)
                {

                    db.Instituicaos.Add(instituicao);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }

                return View(instituicao);
            
        }

        // GET: Instituicoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Instituicao instituicao = db.Instituicaos.Find(id);
                if (instituicao == null)
                {
                    return HttpNotFound();
                }
                return View(instituicao);
            }
        }

        // POST: Instituicoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InstituicaoId,InstituicaoLogin,InstituicaoSenha,Status,InstituicaoNome,InstituicaoTelefone,InstituicaoEmail,InstituicaoEndereco")] Instituicao instituicao)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(instituicao).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(instituicao);
            }
        }

        // GET: Instituicoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Instituicao instituicao = db.Instituicaos.Find(id);
                if (instituicao == null)
                {
                    return HttpNotFound();
                }
                return View(instituicao);
            }
        }

        // POST: Instituicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                Instituicao instituicao = db.Instituicaos.Find(id);
                db.Instituicaos.Remove(instituicao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind(Include = "InstituicaoLogin,InstituicaoSenha")]Instituicao instituicao, bool chkConectado)
        {
            idsessao = db.Instituicaos.FirstOrDefault(x => x.InstituicaoLogin.Equals(instituicao.InstituicaoLogin) && x.InstituicaoSenha.Equals(instituicao.InstituicaoSenha));

            if (idsessao != null)
            {
                Session["LoginAdministrador"] = idsessao.InstituicaoId;
                return RedirectToAction("Index", "Instituicoes");
            }
            else if (idsessao != null)
            {
                Session["LoginAdministrador"] = idsessao.InstituicaoId;
                return RedirectToAction("Index", "Instituicoes");
            }
            else
            {
                ModelState.AddModelError("", "Login ou senha não coincidem!");
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["LoginAdministrador"] = null;
            Session["LoginInstituicao"] = null;
            return RedirectToAction("Login", "Instituicoes");
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
