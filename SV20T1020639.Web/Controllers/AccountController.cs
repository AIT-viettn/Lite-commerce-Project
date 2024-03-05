using Microsoft.AspNetCore.Mvc;

namespace SV20T1020639.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            if(Request.Method == "POST")
            {
                return RedirectToAction("Index", "Home");
            }    
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
