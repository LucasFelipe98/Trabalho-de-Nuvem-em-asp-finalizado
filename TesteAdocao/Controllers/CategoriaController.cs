using System;
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
    public class CategoriasController : Controller
    {
        private TesteAdocaoContext db = new TesteAdocaoContext();

        // GET: Categorias
        public ActionResult Index()
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                int id = Convert.ToInt32(Session["LoginAdministrador"]);
                return View(db.Categorias.Where(x => x.InstituicaoId == id).ToList());
            }
        }

        // GET: Categorias/Details/5
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
                Categoria categoria = db.Categorias.Find(id);
                if (categoria == null)
                {
                    return HttpNotFound();
                }
                return View(categoria);
            }
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                return View();
            }
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoriaId,CategoriaNome,CategoriaDescricao")] Categoria categoria)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    categoria.InstituicaoId = Convert.ToInt32(Session["LoginAdministrador"]);
                    db.Categorias.Add(categoria);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(categoria);
            }
        }

        // GET: Categorias/Edit/5
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
                Categoria categoria = db.Categorias.Find(id);
                if (categoria == null)
                {
                    return HttpNotFound();
                }
                return View(categoria);
            }
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoriaId,CategoriaNome,CategoriaDescricao")] Categoria categoria)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    int ida = Convert.ToInt32(Session["LoginAdministrador"]);
                    categoria.InstituicaoId = Convert.ToInt32(Session["LoginAdministrador"]);
                    db.Entry(categoria).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(categoria);
            }
        }

        // GET: Categorias/Delete/5
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
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
                }
        }

        // POST: Categorias/Delete/5
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
                Categoria categoria = db.Categorias.Find(id);
                db.Categorias.Remove(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
