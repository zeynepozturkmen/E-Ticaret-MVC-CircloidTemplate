using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_CircloidTemplate.Models;

namespace MVC_CircloidTemplate.Controllers
{
    public class SupplierController : Controller
    {


        NorthwindEntities db = new NorthwindEntities();


        public SupplierController()
        {
            ViewBag.SupplierSelected = "selected";
        }


        // GET: Supplier
        public ActionResult Index()
        {

            List<Supplier> supList = db.Suppliers.ToList();


            return View(supList);
        }


        public ActionResult AddSupplier()
        {
        

            return View();
        }

        [HttpPost]
        public ActionResult AddSupplier(Supplier sup)
        {
            db.Suppliers.Add(sup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //public ActionResult DeleteSupplier(int supID)
        //{
        //    Supplier sup = db.Suppliers.Where(x => x.SupplierID == supID).FirstOrDefault();

        //    return View(sup);
        //}


        //Bu methodun içinde oluşan hata Ajax'ı etkilemez.Ajax için success Ajax'ın doğru bir şekilde action'a ulaşmış olmasıyla ilgilidir.
        //Bu methodda veri tabanındaki ilişkilerden dolayı kayıt silinemez ve benzeri hatalar Ajax'ı ilgilendirmez.
        //


        //[HttpPost]
        //public ActionResult DeleteSupplier(int id)
        //{
        //    try
        //    {
        //        Supplier sp = db.Suppliers.Where(x => x.SupplierID == id).FirstOrDefault();
        //        db.Suppliers.Remove(sp);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    catch (Exception)
        //    {
        //        return RedirectToAction("DeleteSupplier");
        //    }
        //}



        [HttpPost]
        public string DeleteSupplier(int id)
        {
            try
            {
                Supplier sp = db.Suppliers.Find(id);
                db.Suppliers.Remove(sp);
                db.SaveChanges();
                return "OK";
            }

            catch (Exception)
            {
                return "Error";
            }      
        }



        public ActionResult UpdateSupplier(int id)
        {

            Supplier sup = db.Suppliers.Where(x => x.SupplierID == id).FirstOrDefault();
            return View(sup);

        }

        [HttpPost]
        public ActionResult UpdateSupplier([Bind(Include="SupplierID,CompanyName,ContactName,ContactTitlie,Address,City,Region,PostalCode,County,Phone")]Supplier s)
        {
            if (ModelState.IsValid)
            {

                db.Entry(s).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

                return RedirectToAction("Index");

            }

            return RedirectToAction("UpdateSupplier", new { id = s.SupplierID });

        }

    }


}