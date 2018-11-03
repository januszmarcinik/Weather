using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weather.Domain.Abstract;
using Weather.Domain.Entities;
using Weather.WebUI.Models;

namespace Weather.WebUI.Controllers
{
    public class UserController : Controller
    {
        IUserRepository userRepository;

        public UserController(IUserRepository userRepo)
        {
            userRepository = userRepo;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            User user = userRepository.Users.FirstOrDefault(x => x.Login == model.Login && x.Password == model.Password);
            if (user == null)
            {
                TempData["LoginError"] = "Nieprawidłowa nazwa użytkownika i/lub hasło. Spróbuj ponownie.";
                return View();
            }
            else
            {
                Session["User"] = user;
                return RedirectToAction("Index", "Weather");
            }
        }

        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            User userExist = userRepository.Users.FirstOrDefault(x => x.Login == model.Login);
            if (userExist != null)
            {
                TempData["LoginError"] = "Użytkownik o podanym loginie istnieje już bazie.";
                return View();
            }

            if (model.Password != model.ConfirmPassword)
            {
                TempData["LoginError"] = "Podane hasła różnią się od siebie.";
                return View();
            }

            User user = new User()
            {
                Login = model.Login,
                Password = model.Password
            };
            userRepository.AddUser(user);
            Session["User"] = user;
            return RedirectToAction("Index", "Weather");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Weather");
        }
    }
}