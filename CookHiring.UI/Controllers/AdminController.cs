using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CookHiringAspNet.Models;
using CookHiringAspNet.Controllers;


namespace CookHiringAspNet.Controllers
{
    public class AdminController : Controller
    {
        private CookHiringDBEntities1 db = new CookHiringDBEntities1();
        private UsersController usersController = new UsersController();
        public ActionResult Index()
        {
            var jobSeekerCount = db.Users.Where(u => u.userRole == "jobseeker").Count();
            var jobProviderCount = db.Users.Where(u => u.userRole == "jobprovider").Count();
            var jobsCount = db.jobs.Count();
            ViewBag.JobSeekerCount = jobSeekerCount;
            ViewBag.JobProviderCount = jobProviderCount;
            ViewBag.JobCount = jobsCount;
            var totalSelected = db.appliedJobs.Where(u => u.isGotJob == 1).Count();
            var totalWaiting = db.appliedJobs.Where(u => u.isGotJob == 0).Count();
            var totaRejected = db.appliedJobs.Where(u => u.isGotJob == 2).Count();
            ViewBag.TotalSelected = totalSelected;
            ViewBag.TotalWaiting = totalWaiting;
            ViewBag.TotaRejected = totaRejected;
            return View();
        }
        public ActionResult AllJobSeekers()
        {
            var users = db.Users.Where(u => u.userRole == "jobseeker");
            return View(users.ToList());
        }
        public ActionResult AllJobProviders()
        {
            var users = db.Users.Where(u => u.userRole == "jobprovider");
            return View(users.ToList());
        }

        public ActionResult AllJobsList()
        {
            var jobs = db.jobs;
            return View(jobs.ToList());
        }
        //Edit
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
        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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

        //Create User
        public ActionResult Create(User signupDetails)
        {
            int id = 0;
            TempData["val"] = "";
            var lastUser = db.Users.SingleOrDefault(u => u.email == signupDetails.email);
            try
            {
                if (lastUser == null && signupDetails != null)
                {
                    lastUser = db.Users.OrderByDescending(u => u.id).FirstOrDefault();
                    User user = new User();
                    if (lastUser == null)
                    {
                        user.id = id;
                    }
                    else
                    {
                        user.id = lastUser.id + 1;
                    }
                    user.email = signupDetails.email;
                    user.password = signupDetails.password;
                    user.mobileNumber = signupDetails.mobileNumber;
                    user.userRole = signupDetails.userRole;
                    db.Users.Add(user);
                    db.SaveChanges();
                    TempData["val"] = "Success";
                }
                else
                {
                    TempData["val"] = "Email already exists";
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        //Job create
        //Create User
        public ActionResult CreateJob(job jobDetails)
        {
            int id = 0;
            job job = new job();
            var lastJob = db.jobs.OrderByDescending(u => u.id == id).FirstOrDefault();
            if (lastJob != null)
            {
                job.id = lastJob.id + 1;
            }
            else
            {
                job.id = id;
            }
                job.jobDescription = jobDetails.jobDescription;
                job.jobLocation = jobDetails.jobLocation;
                job.fromDate = jobDetails.fromDate;
                job.toDate = jobDetails.toDate;
                job.mobileNumber = jobDetails.mobileNumber;
                job.jobProviderId = jobDetails.jobProviderId;
                job.wagePerDay = jobDetails.wagePerDay;
                job.jobStatus = 0;
            return View();
        }

        //Edit
        public ActionResult EditJob(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJob(job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.jobSeekerProfiles, "jobSeekerId", "name", job.id);
            return View(job);
        }
        //Job delete
        public ActionResult DeleteJob(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteJobConfirmed(int id)
        {
            job job = db.jobs.Find(id);
            db.jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Job Details
        // GET: Users/Details/5
        public ActionResult JobDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
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
