﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using GesCollege.Models;
using GesCollege.PlutoCollege;

namespace GesCollege.Controllers
{
    public class DepartementController : Controller
    {
        private CollegeContext db = new CollegeContext();

        // GET: Departement
        public ActionResult Index()
        {
            var departements = db.Departements.Include(d => d.College).Include(d=>d.Directeur);
            return View(departements.ToList());
        }

        // GET: Departement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departement departement = db.Departements.Find(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            return View(departement);
        }

        // GET: Departement/Create
        public ActionResult Create()
        {
            ViewBag.CodeCollege = new SelectList(db.Colleges, "Id", "Nom");
           // ViewBag.Directeur = new SelectList(db.Enseignants, "Id", "Nom");
            return View();
        }

        // POST: Departement/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NomDep,CodeCollege")] Departement departement)
        {
            if (ModelState.IsValid)
            {
                db.Departements.Add(departement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodeCollege = new SelectList(db.Colleges, "Id", "Nom", departement.CodeCollege);
           // ViewBag.Directeur = new SelectList(db.Enseignants, "Id", "Nom", departement.Directeur);
            return View(departement);
        }

        // GET: Departement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departement departement = db.Departements.Find(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodeCollege = new SelectList(db.Colleges, "Id", "Nom", departement.CodeCollege);
            return View(departement);
        }

        // POST: Departement/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NomDep,CodeCollege")] Departement departement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodeCollege = new SelectList(db.Colleges, "Id", "Nom", departement.CodeCollege);
            return View(departement);
        }

        // GET: Departement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departement departement = db.Departements.Find(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            return View(departement);
        }

        // POST: Departement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Departement departement = db.Departements.Find(id);
            db.Departements.Remove(departement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
        public ActionResult EtatDepartement()
        {
            List<Departement> allDepatement = new List<Departement>();
            allDepatement = db.Departements.ToList();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report/DepartementReport.rpt")));
            rd.SetDataSource(db.Departements.Select(d => new
            {
                College = d.College.Nom,
                nomDep = d.NomDep,
                nom = d.Directeur.Nom,
                prenom = d.Directeur.Prenom
            }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ListDepartement.pdf");
        }
    }
}
