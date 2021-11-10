using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Travel.Models;

namespace Travel.Controllers
{
    public class OffersController : Controller
    {
        private TravelContext db = new TravelContext();

        // GET: Offers
        public ActionResult Index()
        {
            return View(db.o.ToList());
        }

        // GET: Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offers offers = db.o.Find(id);
            if (offers == null)
            {
                return HttpNotFound();
            }
            return View(offers);
        }

        // GET: Offers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Thumbnail,Title,Detail")] Offers offers, HttpPostedFileBase file)
        {
            //if (ModelState.IsValid)
            //{
            //    db.o.Add(offers);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(offers);
            try
            {
                // TODO: Add insert logic here
                using (TravelContext db = new TravelContext())
                {
                    if (file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.Ticks;
                        string fileExt = Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/img"), fileName + fileExt);
                        file.SaveAs(path);

                        offers.Thumbnail = fileName + fileExt;

                    }
                    ViewBag.MsgSuccess = "File Uploaded Successfully";

                    db.o.Add(offers);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Offers", new { status = "Data Uploaded" });
                }
            }
            catch
            {
                return View("Index","Offers", new { status = "Uploading Failed" });
            }
        }

        // GET: Offers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offers offers = db.o.Find(id);
            if (offers == null)
            {
                return HttpNotFound();
            }
            return View(offers);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Thumbnail,Title,Detail")] Offers offers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offers);
        }

        // GET: Offers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offers offers = db.o.Find(id);
            if (offers == null)
            {
                return HttpNotFound();
            }
            return View(offers);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offers offers = db.o.Find(id);
            db.o.Remove(offers);
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
