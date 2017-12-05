using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TesteAdocao.Models;

namespace TesteAdocao.Controllers
{
    public class AnimalsController : Controller
    {
        private TesteAdocaoContext db = new TesteAdocaoContext();
        private static int? idanimal;
        private static int? idCategoria;
        private static int apoio;
        // GET: Animals
        public ActionResult Index()
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                int id = Convert.ToInt32(Session["LoginAdministrador"]);
                return View(db.Animals.Where(x => x.InstituicaoId == id).ToList());
            }
        }

        public ActionResult AnimalPorCategoria(int id)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                int ida = Convert.ToInt32(Session["LoginAdministrador"]);
                var animals = db.Animals.Include(a => a.Categoria).Where(x => x.CategoriaId == id);
                return View(db.Animals.Where(x => x.InstituicaoId == ida).ToList());
            }
        }

        // GET: Animals/Details/5
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
                Animal animal = db.Animals.Find(id);
                if (animal == null)
                {
                    return HttpNotFound();
                }
                return View(animal);
            }
        }

        // GET: Animals/Create
        public ActionResult Create()
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                int ida = Convert.ToInt32(Session["LoginAdministrador"]);

                var categorias = db.Categorias.Where(x => x.InstituicaoId == ida);

                ViewBag.CategoriaId = new SelectList(categorias, "CategoriaId", "CategoriaNome");
                return View();
            }
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnimalId,AnimalNome,AnimalDescricao,AnimalCor,AnimalRaca,AnimalImagem,CategoriaId")] Animal animal)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                int ida = Convert.ToInt32(Session["LoginAdministrador"]);

                if (ModelState.IsValid)
                {
                    animal.InstituicaoId = ida;
                    db.Animals.Add(animal);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                var categorias = db.Categorias.Where(x => x.InstituicaoId == ida);
                ViewBag.CategoriaId = new SelectList(categorias, "CategoriaId", "CategoriaNome", animal.CategoriaId);
                return View(animal);
            }
        }

        // GET: Animals/Edit/5
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
                Animal animal = db.Animals.Find(id);
                if (animal == null)
                {
                    return HttpNotFound();
                }
                int ida = Convert.ToInt32(Session["LoginAdministrador"]);
                ViewBag.CategoriaId = new SelectList(db.Categorias.Where(x => x.InstituicaoId == ida), "CategoriaId", "CategoriaNome", animal.CategoriaId);
                return View(animal);
            }
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnimalId,AnimalNome,AnimalDescricao,AnimalCor,AnimalRaca,CategoriaId")] Animal animal)
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                if (ModelState.IsValid)
                {
                   
                    animal.InstituicaoId = Convert.ToInt32(Session["LoginAdministrador"]);
                    db.Entry(animal).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                int ida = Convert.ToInt32(Session["LoginAdministrador"]);
                ViewBag.CategoriaId = new SelectList(db.Categorias.Where(x => x.InstituicaoId == ida), "CategoriaId", "CategoriaNome", animal.CategoriaId);
                return View(animal);
            }
        }

        // GET: Animals/Delete/5
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
                Animal animal = db.Animals.Find(id);
                if (animal == null)
                {
                    return HttpNotFound();
                }
                return View(animal);
            }
        }

        // POST: Animals/Delete/5
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
                Animal animal = db.Animals.Find(id);
                db.Animals.Remove(animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult FileUpload()
        {
            if (Session["LoginAdministrador"] == null)
            {
                return RedirectToAction("Login", "Instituicao");
            }
            else
            {
                int arquivosSalvos = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase arquivo = Request.Files[i];

                    //Suas validações ......

                    //Salva o arquivo
                    if (arquivo.ContentLength > 0)
                    {
                        var uploadPath = Server.MapPath("~/Content/Uploads");
                        string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(arquivo.FileName));

                        arquivo.SaveAs(caminhoArquivo);
                        arquivosSalvos++;
                    }
                }

                ViewData["Message"] = String.Format("{0} arquivo(s) salvo(s) com sucesso.",
                    arquivosSalvos);
                return View("Upload");
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
