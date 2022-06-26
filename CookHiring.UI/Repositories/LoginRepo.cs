using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using CookHiringAspNet.Models;



namespace CookHiringAspNet.Repositories

{

    public class LoginRepo

    {

        private CookHiringDBEntities1 db = new CookHiringDBEntities1();

        public String Signup(User signupDetails)

        {

            int id = 0;

            String res = "";

            var lastUser = db.Users.SingleOrDefault(u => u.email == signupDetails.email);

            try

            {

                if (lastUser == null && signupDetails != null)

                {

                    lastUser = db.Users.OrderByDescending(u => u.id).FirstOrDefault();

                    jobSeekerProfile jsp = new jobSeekerProfile();

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

                    jsp.email = user.email;

                    jsp.jobSeekerId = user.id;

                    jsp.experience = "";

                    jsp.mobileNumber = user.mobileNumber;

                    jsp.address = "";

                    jsp.name = "";

                    db.Users.Add(user);

                    db.SaveChanges();

                    db.jobSeekerProfiles.Add(jsp);

                    db.SaveChanges();

                    res = "0";

                }

                else

                {

                    res = "Email already exists";

                }

            }

            catch (Exception ex)

            {



            }

            return res;

        }

        public User Login(User loginDetails)

        {

            try

            {

                var user = db.Users.FirstOrDefault(u => u.email == loginDetails.email && u.password == loginDetails.password);

                if (user == null)

                    return null;

                else

                    return user;

            }

            catch (Exception ex)

            {

                return null;

            }

        }

    }

}
