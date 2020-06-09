using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AFM_T.Models;

namespace AFM_T.Controllers
{
    public class AFM_ProductController : Controller
    {
        private AFMEntities3 db = new AFMEntities3();

        // GET: AFM_Product
        public ActionResult Index()
        {
            return View(db.AFM_Product.ToList());
        }

        // GET: AFM_Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AFM_Product aFM_Product = db.AFM_Product.Find(id);
            if (aFM_Product == null)
            {
                return HttpNotFound();
            }
            return View(aFM_Product);
        }

        // GET: AFM_Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AFM_Product/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RowId,ProductID,ProductName,UnitPathId,Item,Amount,Size,Kind,OriginalHolder,Worth,Material,ObjectTime,Maker,Status,Place,ImageUrl,Remark,Remark2,CreateDate,CreateUserId,UpdateDate,UpdateUserId,Cancel")] AFM_Product aFM_Product)
        {
            if (ModelState.IsValid)
            {
                db.AFM_Product.Add(aFM_Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aFM_Product);
        }

        // GET: AFM_Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AFM_Product aFM_Product = db.AFM_Product.Find(id);
            if (aFM_Product == null)
            {
                return HttpNotFound();
            }
            return View(aFM_Product);
        }

        // POST: AFM_Product/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RowId,ProductID,ProductName,UnitPathId,Item,Amount,Size,Kind,OriginalHolder,Worth,Material,ObjectTime,Maker,Status,Place,ImageUrl,Remark,Remark2,CreateDate,CreateUserId,UpdateDate,UpdateUserId,Cancel")] AFM_Product aFM_Product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aFM_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aFM_Product);
        }

        // GET: AFM_Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AFM_Product aFM_Product = db.AFM_Product.Find(id);
            if (aFM_Product == null)
            {
                return HttpNotFound();
            }
            return View(aFM_Product);
        }

        // POST: AFM_Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AFM_Product aFM_Product = db.AFM_Product.Find(id);
            db.AFM_Product.Remove(aFM_Product);
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
