using CookHiring.Data;
using CookHiring.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CookHiring.UI.Controllers
{
    
    public class UserController : Controller
    {
        // GET: User
        public UserRepository userRepository;
        public UserController()
        {
            userRepository = new UserRepository();
        }


        public ActionResult Index()
        {
            List<User> users = userRepository.GetUsers();
            return View(users);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            userRepository.AddUser(user);
            return RedirectToAction("Index");
            }
        public ActionResult Details(int id)
        {
           User user= userRepository.GetUserById(id);
            return View(user);
        }
        public ActionResult Edit(int id)
        {
            User user = userRepository.GetUserById(id);
            return View(user);
        }
        [HttpPut]
        public ActionResult Edit(User data)
        { 
            userRepository.EditUser(data);
            return RedirectToAction("Index");
        }
       
        public ActionResult Delete(int id)
        {
            userRepository.DeleteUser(id);
            return RedirectToAction("Index");
        }
        public ActionResult Index()

        {

            var users = db.Users.Include(u => u.jobSeekerProfile);

            return View(users.ToList());

        }



        // GET: Users/Details/5

        public ActionResult Details(int? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            User user = db.Users.Find(id);

            if (user == null)

            {

                return HttpNotFound();

            }

            return View(user);

        }

        // GET: Users/Edit/5

        public ActionResult Edit(int? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            User user = db.Users.Find(id);

            if (user == null)

            {

                return HttpNotFound();

            }

            ViewBag.id = new SelectList(db.jobSeekerProfiles, "jobSeekerId", "name", user.id);

            return View(user);

        }



        // POST: Users/Edit/5

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "id,email,password,mobileNumber,userRole")] User user)

        {

            if (ModelState.IsValid)

            {

                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");

            }

            ViewBag.id = new SelectList(db.jobSeekerProfiles, "jobSeekerId", "name", user.id);

            return View(user);

        }

    }
}
