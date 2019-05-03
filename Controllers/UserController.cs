using MVC_CircloidTemplate.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
        //[Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        // GET: User
       

        public UserController() {
            ViewBag.UserSelected = "selected";
        }

        public ActionResult Index()
        {
            //Veri tabanındaki tüm kullanıcıları alarak users isimli collection'da toplayacak.
            MembershipUserCollection users = Membership.GetAllUsers();
            return View(users);

        }

        public ActionResult AddUser(){

            return View();
        }

        [HttpPost]
        public ActionResult AddUser(UserClass uc)
        {
            Membership.CreateUser(uc.UserName, uc.Password,uc.Email,uc.PasswordQuestion, uc.PasswordAnswer,
                true,out MembershipCreateStatus status);

            string createmessage = "";

            switch (status)
            {
                case MembershipCreateStatus.Success: //basarılı   
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    createmessage = "Gerçersiz kullanıcı adı.";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    createmessage = "Gerçersiz şifre adı.";
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    createmessage = "Gerçersiz gizli soru ";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    createmessage = "Gerçersiz gizli cevap";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    createmessage = "Gerçersiz email";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    createmessage = "Kullanılmıs kullanıcı adı.";
                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    createmessage = "Kullanılmış email adresi";
                    break;
                case MembershipCreateStatus.UserRejected:
                    createmessage = "Kullanıcı engellendi";
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                   createmessage = "Geçersiz kullanıcı anahtarı";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    createmessage = "Tekrarlanmış kullanıcı anahtarı";
                    break;
                case MembershipCreateStatus.ProviderError:
                    createmessage = "Sağlayıcı hatası.";
                    break;
                default:
                    break;
            }

            ViewBag.createMessage = createmessage;

            if(createmessage== "")
            {
                return RedirectToAction("Index"); //hata yoksa index sayfasında kaydedilen kullanıcıları göstericek
            }
            else
            {
                return View(); //eger hata varsa oldugu sayfada kalsın
            }

        }


        public ActionResult AssignRole(string username,string message = null)
        {

            /*Parametre olarak id yazmak zorundayız,sebebi projenin App_Start klasörünün altınsa Route_Config.cs dosyasında
             * "{controller}/{action}/{id}" bu parametre adının default adı id olduğu için
             * parametre adınında id olması gerekiyor.
              Kullanıcıya rol ataya tıkladığında 
              kullanıcı adını parametre olarak buraya alıyoruz
              buradan da kullanıcının adını viewe gönderiyoruz
              amacımız parametre bilgisini view a taşımak.
              View tarafında ekle butonuna basınca tekrar kullanıcı adını ve rol adını viewden alıp post tarafına taşımak.
              */

            if (string.IsNullOrWhiteSpace(username))
            
                return RedirectToAction("Index");

            MembershipUser user = Membership.GetUser(username);
            if (user == null)
            {

                return HttpNotFound();
            }

            string[] userRoles = Roles.GetRolesForUser(username);
            string[] allRoles = Roles.GetAllRoles();

            List<string> availableRoles = new List<string>();
            foreach (string role in allRoles)
            {
                if (!userRoles.Contains(role))
                    availableRoles.Add(role);

            }
            ViewBag.AvailableRoles = availableRoles;
            ViewBag.UserRoles = userRoles;
            ViewBag.Username = username;
            ViewBag.Message = message;

            return View();
        }


        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            Membership.DeleteUser(id);

            return View();
        }

        [HttpPost]
        public ActionResult AssignRole(string username, List<string> addedRoles)
        {
            if (addedRoles == null)
            
                return RedirectToAction("AssignRole", new { username = username, message = "Önce Rol seçiniz." });

            
            if (addedRoles.Count < 1)
                return RedirectToAction("AssignRole", new { username = username, message = "Hata" });
                 Roles.AddUserToRoles(username, addedRoles.ToArray());

            return RedirectToAction("AssignRole", new { username = username, message = "Başarılı" });

        }

        [HttpPost]
        public string DeleteRole(string username,string removedRoles)
        {
            string[] removedRolesArray = removedRoles.Split(',');
            if (removedRolesArray.Length < 1 || string.IsNullOrWhiteSpace(removedRolesArray[0]))
                return "Hata";
            Roles.RemoveUserFromRoles(username, removedRolesArray);
            return "Basarili";
        }

       
        public string UsersRoles(string username)
        {

            string roles = " ";
            List<string> role = Roles.GetRolesForUser(username).ToList();

            foreach (string item in role)
            {
                roles += item + " , ";
            }


            //ViewBag.UserRoles = userRoles;


            return roles;
        }

        //[HttpPost]
        //[ActionName("UsersRoles")]
        public ActionResult UsersRoles2(string username)
        {

            List<string> role = Roles.GetRolesForUser(username).ToList();
            ViewBag.UserRoles = role;
            return View();
        }

    }
}