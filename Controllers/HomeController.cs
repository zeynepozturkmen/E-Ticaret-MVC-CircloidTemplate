using MVC_CircloidTemplate.App_Classes;
using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        NorthwindEntities db = new NorthwindEntities();

        public HomeController()
        {
            ViewBag.MainPageSelected = "selected";
        }


        public ActionResult Index()
        {

            ViewBag.ActiveUserCount = HttpContext.Application["ActiveUserCount"];
            ViewBag.TotalUserCount = HttpContext.Application["TotalUserCount"];

            return View();
        }

        public ActionResult AssignCookie()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AssignCookie(string CookieName,string CookieValue)
        {
            HttpCookie hc = new HttpCookie(CookieName);
            hc.Value = CookieValue;
            hc.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(hc);

            return View();

        }

        public ActionResult RetrieveCookie()
        {
            string cookieVal=Request.Cookies["SC201-Morning"].Value;
            ViewBag.CookieValue = cookieVal;
            return View();


        }

        public ActionResult MyCart()
        {
            //List<Product> myCartList =new List<Product>();

            CartClass crt;
            if (Session["CurrentCart"] != null)
            {
                crt = (CartClass)Session["CurrentCart"];
                //myCartList = crt;
            }
            else
            {
                crt =new CartClass();
            }

            Session["CurrentCart"] = crt;
            return View();

        }


        [HttpPost]
        public string RemoveCart(int id)
        {

            //Sepet varsa,gelen ürünün varolan sepete ekle,sepet yoksa önce aktif session için(oturum) için sepet oluştur ve sepete gelen ürünü ekle

            string cartMessage = "";

            CartClass crt = (CartClass)Session["CurrentCart"];
            Product prd = db.Products.FirstOrDefault(x => x.ProductID == id); //Producttan o id'li ürünü bul
            //crt.PrdList.Remove(prd);



            crt.PrdList.RemoveAll(x => x.ProductID ==id);

            Session["CurrentCart"] = crt;

            cartMessage = "Ürün sepetten çıkarılmıştır. ";

            return cartMessage;
          
        }

        public ActionResult PartialCartListView()
        {
            if (Session["CurrentCart"] != null)
            {
                CartClass c = (CartClass)Session["CurrentCart"];
                // int n = c.PrdList.Count();
                return PartialView(c.PrdList);
            }
            return PartialView();

        }

    }
}