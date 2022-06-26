using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Mvc;

using CookHiringAspNet.Models;

using System.Data.Entity;

using System.Net;



namespace CookHiringAspNet.Controllers

{

    public class HomeController : Controller

    {

        CookHiringDBEntities1 db = new CookHiringDBEntities1();

        public ActionResult Index()

        {

            ViewBag.jid = "";

            ViewBag.jpid = "";

            var jobs = db.jobs.Include(j => j.User);

            //ViewBag.user = Session["userRole"].ToString();

            return View(jobs.ToList());

            

        }

        //jobseekerprofileEdit

        public ActionResult Edit(int? id)

        {

            AddJobModel ajm = new AddJobModel();

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            ajm.jsp = db.jobSeekerProfiles.Find(id);

            if (ajm.jsp == null)

            {

                return HttpNotFound();

            }

            ViewBag.jobSeekerId = new SelectList(db.Users, "id", "email", ajm.jsp.jobSeekerId);

            return View(ajm);

        }



        // POST: jobSeekerProfiles/Edit/5

        // To protect from overposting attacks, enable the specific properties you want to bind to, for 

        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Edit(AddJobModel addJobModel)

        {

            if (ModelState.IsValid)

            {

                HomeController hc = new HomeController();

                db.Entry(addJobModel.jsp).State = EntityState.Modified;

                db.SaveChanges();

                appliedJob aj = new appliedJob();

                aj.isGotJob = 0;

                aj.jobId = addJobModel.ajs.jobId;

                aj.jobProviderId = addJobModel.ajs.jobProviderId;

                aj.jobSeekerId = Convert.ToInt32(Session["userId"]);

                db.appliedJobs.Add(aj);

                db.SaveChanges();

                return RedirectToAction("Index", "Home");

            }

            ViewBag.jobSeekerId = new SelectList(db.Users, "id", "email", addJobModel.jsp.jobSeekerId);

            return RedirectToAction("~/Home");

        }

    }

}
