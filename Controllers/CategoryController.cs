using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_CircloidTemplate.Models;

namespace MVC_CircloidTemplate.Controllers
{
    public class CategoryController : Controller
    {


        NorthwindEntities db = new NorthwindEntities();


        public CategoryController()
        {
            ViewBag.CategorySelected = "selected";
        }

        // GET: Category
        public ActionResult Index()
        {

            List<Category> ctgList = db.Categories.ToList();


            return View(ctgList);
        }

        public ActionResult AddCategory() {

            return View();
        }

        [HttpPost]
        public ActionResult AddCategory([Bind(Include = "CategoryName,Description")] Category cat, HttpPostedFileBase Picture)
        {
            if (Picture == null)
            {
                return View();
            }

            cat.Picture = ConvertToBytes(Picture);


            db.Categories.Add(cat);
            db.SaveChanges();


            return RedirectToAction("Index");
        }



        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes(image.ContentLength);
            byte[] bytes = new byte[imageBytes.Length + 78];
            Array.Copy(imageBytes, 0, bytes, 78, imageBytes.Length);

            return bytes;
        }


        //public ActionResult DeleteCategory(int id)
        //{

        //    Category ctg = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
        //    return View(ctg);
        //}


        [HttpPost]
        public ActionResult DeleteCategory(int? id)
        {
            Category category = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
            db.Categories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult UpdateCategory(int id)
        {

            Category ctg = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
            return View(ctg);
           
        }


        [HttpPost]
        public ActionResult UpdateCategory([Bind(Include ="CategoryID,CategoryName,Description")] Category ktg,HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                Category k = db.Categories.Find(ktg.CategoryID);
                k.CategoryName = ktg.CategoryName;
                k.Description = ktg.Description;
                if (Picture != null)
                {
                    k.Picture = ConvertToBytes(Picture);
                }
            }

           

           
            db.SaveChanges();

            return RedirectToAction("Index");


        }

    }
}