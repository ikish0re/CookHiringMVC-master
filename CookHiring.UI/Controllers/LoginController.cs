using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Mvc;

using System.Web.Security;

using CookHiringAspNet.Models;

using CookHiringAspNet.Repositories;



namespace CookHiringAspNet.Controllers

{

    public class LoginController : Controller

    {

        private CookHiringDBEntities1 db = new CookHiringDBEntities1();

        private LoginRepo loginRepo = new LoginRepo();

        public ActionResult Signup(User signupDetails)

        {

            var userRoles = new List<string>() { "admin", "jobseeker", "jobprovider" };

            ViewBag.UserRoles = userRoles;

            TempData["pass"] = "";

            if (signupDetails.password == signupDetails.repeatPassword)

            {

                TempData["val"] = loginRepo.Signup(signupDetails);

                if (TempData["val"] == "0")

                    return RedirectToAction("Login", "Login");

                else

                    return View();

            }

            else

            {

                TempData["pass"] = "Password and Repeat Password Does Not Matched";

                return View();

            }

        }

        public ActionResult Login(User loginDetails)

        {

            var user = loginRepo.Login(loginDetails);

            if (user != null)

            {

                Session["userId"] = user.id;

                Session["userRole"] = user.userRole;

                if (user.userRole == "admin")

                {

                    Session["user"] = "admin";

                    return RedirectToAction("Index", "Admin");

                }

                else if (user.userRole == "jobseeker")

                {

                    Session["user"] = "jobseeker";

                    return RedirectToAction("Index", "Home");

                }

                else

                {

                    Session["user"] = "jobprovider";

                    return RedirectToAction("Index", "Home");

                }

            }

            else

            {

                return View();

            }

        }

        public ActionResult Logout()

        {

            Session.Abandon();

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home", "Home");

        }

    }

}
