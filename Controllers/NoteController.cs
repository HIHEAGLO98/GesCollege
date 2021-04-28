﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GesCollege.Models;
using GesCollege.PlutoCollege;

namespace GesCollege.Controllers
{
    public class NoteController : Controller
    {
        private CollegeContext db = new CollegeContext();

        // GET: Note
        public ActionResult Index()
        {
            var notes = db.Notes.Include(n => n.Courses).Include(n => n.Etudiants);
            return View(notes.ToList());
        }

        // GET: Note/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "LibCours");
            ViewBag.IdEtudiant = new SelectList(db.Etudiants, "Id", "Nom");
            return View();
        }

        // POST: Note/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NoteControle,IdCourse,IdEtudiant")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Notes.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "LibCours", note.IdCourse);
            ViewBag.IdEtudiant = new SelectList(db.Etudiants, "Id", "Nom", note.IdEtudiant);
            return View(note);
        }

        // GET: Note/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "LibCours", note.IdCourse);
            ViewBag.IdEtudiant = new SelectList(db.Etudiants, "Id", "Nom", note.IdEtudiant);
            return View(note);
        }

        // POST: Note/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NoteControle,IdCourse,IdEtudiant")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCourse = new SelectList(db.Courses, "Id", "LibCours", note.IdCourse);
            ViewBag.IdEtudiant = new SelectList(db.Etudiants, "Id", "Nom", note.IdEtudiant);
            return View(note);
        }

        // GET: Note/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.Notes.Find(id);
            db.Notes.Remove(note);
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
    }
}
