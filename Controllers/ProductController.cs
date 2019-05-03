using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_CircloidTemplate.App_Classes;
using MVC_CircloidTemplate.Models;

namespace MVC_CircloidTemplate.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product

        NorthwindEntities db = new NorthwindEntities();


        public ProductController()
        {
            ViewBag.ProductSelected = "selected";
        }


        public ActionResult Index()


        {
            List<Product> prdList = db.Products.ToList();

            return View(prdList);
        }



        public ActionResult AddProduct()
        {
            ViewBag.catList=db.Categories.ToList();
            ViewBag.supList=db.Suppliers.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product p)
        {

            db.Products.Add(p);
            db.SaveChanges();


            return RedirectToAction("Index");
        }


        //Sil işlemini üç farklıı yolla yapacağız.
        //İlk çözümümüz sil butonuna basılınca yeni bir view açılacak yani kullanıcı yeni bir sayfaya yönlendirilecek ve evet derse silinecek.
        //İkinci yol sil butonuna basılınca yukarıda alert kutusu çıkacak ve kayıt silinsin mi evet denirse silinecek.
        //Bu yöntemin dez avantajı alert kutusu bir kaç kez görüntülendikten sonra browser otomatik olarak alert kutusunun altına 
        //cehcekBox ekliyor ve bu mesajı tekrar gösterme seçeneği sunuyor.Eğer kullanıcı checkBox'ı işaretler
        //tekrar alert kutusu gözükmeyeceği için silme işlemi gerçekleştirilemiyor.(Ajax kodu yazılacak.)
        //3.Yol; Sil butonuna basılınca küçük bir pencere açılacak (başka bir sayfaya yönlendirilmeyecek) evet seçilirse silme işlemi
        //gerçekleştirilecek.Bu işlem için de ajax kodu yazılacak.


        public ActionResult DeleteProduct(int prdID)
        {
            Product prd = db.Products.FirstOrDefault(x => x.ProductID == prdID);
     

            return View(prd);

        }

        [HttpPost]
        public ActionResult DeleteProduct(Product p)
        {
            Product prod = db.Products.FirstOrDefault(x => x.ProductID == p.ProductID);
            db.Products.Remove(prod);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateProduct()
        {
            //    Product prd = db.Products.Where(x => x.ProductID == prdID).FirstOrDefault();

            int productID = Convert.ToInt32(Request.QueryString["prdID"].ToString());
            string productName=Request.QueryString["prdName"].ToString(); //Request=istek
            string productFrom = Request.QueryString["prdFrom"].ToString(); //Request=istek

            Product prd = db.Products.FirstOrDefault(x => x.ProductID == productID);

            ViewBag.catList = db.Categories.ToList();
            ViewBag.supList = db.Suppliers.ToList();
            ViewBag.productFrom = productFrom;
            return View(prd);

        }

        [HttpPost]
        public ActionResult UpdateProduct(Product p)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");     db.SaveChanges();

                Product prd = db.Products.Find(p.ProductID);

                prd.ProductName = p.ProductName;
                prd.UnitPrice = p.UnitPrice;

                prd.UnitsInStock = p.UnitsInStock;

                prd.SupplierID = p.SupplierID;
                prd.CategoryID = p.CategoryID;
                db.SaveChanges();

            }

       

            return RedirectToAction("Index", "Product");

            //return RedirectToAction("UpdateProduct", new { id = p.ProductID });

        }


        [HttpPost]
        public string AddCart(int id)
        {
            //Sepet varsa,gelen ürünün varolan sepete ekle,sepet yoksa önce aktif session için(oturum) için sepet oluştur ve sepete gelen ürünü ekle

            string cartMessage = "";
            CartClass crt;
            if (Session["CurrentCart"]==null)
            {
                crt = new CartClass(); //yeni sepet oluşturuluyor.
            }
            else
            {
                crt = (CartClass)Session["CurrentCart"]; //eğer sepet varsa
             
            }

            foreach (Product p in crt.PrdList)
            {
                if (p.ProductID == id)
                {
                    cartMessage = "Eklemek istediğiniz sepette mevcut.";
                    return cartMessage;
                }
             
            }
            Product prd = db.Products.FirstOrDefault(x => x.ProductID == id); //Producttan o id'li ürünü bul

            crt.PrdList.Add(prd);
            Session["CurrentCart"] = crt;
            cartMessage = "Ürün sepete başarıyla eklendi";

            return cartMessage;
        }

        public ActionResult PartialProductCountNav() {
            CartClass c = (CartClass)Session["CurrentCart"];
            int n = c.PrdList.Count();
            return PartialView(c.PrdList);

           //return PartialView(Session["CurrentCart"] as CartClass);
        }


  

    }
}