using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020639.BusinessLayers;
using SV20T1020639.DomainModels;

namespace SV20T1020639.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = "", string password = "")
        {
            ViewBag.Username = username;
            //Kiểm tra xem thông tin nhập có đủ không 
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập đủ tên và mật khẩu");
                return View();
            }

            //Kiểm tra thông tin đăng nhập có hợp lệ hay không 
            var userAccount = UserAccountService.Authorize(username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }
            //Đăng nhập thành công , tạo dữ liệu để lưu cookie
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = "",
                Roles = userAccount.RoleNames.Split(',').ToList()

            };

            //Thiết lập phiên đăng nhập cho tài khoản
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            TempData["Username"] = username;
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewBag.Title = "Thay đổi mật khẩu";
            string userName = TempData["Username"] as string;
            ViewBag.UserName = userName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword( string userName = "", string oldPassword = "", string newPassword = "", string confirmPassword = "")
        {
            ViewBag.OldPassword = oldPassword;
            ViewBag.NewPassword = newPassword;
            ViewBag.userName = userName;
            ViewBag.confirmPassword = confirmPassword;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError("", "Thông tin không đầy đủ");
            }
           /* if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu mới và xác nhận mật khẩu không khớp nhau");
            }*/
            if (oldPassword == newPassword)
            {
                ModelState.AddModelError("", "Mật khẩu mới không được trùng với mật khẩu cũ");
            }
            else
            {
                bool isChangePassword = UserAccountService.ChangePassword(userName, oldPassword, newPassword);
                if (!isChangePassword)
                {
                    ModelState.AddModelError("", "Thay đổi mật khẩu thất bại");
                }
               
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Thay đổi mật khẩu";
                return View();
            }
            return RedirectToAction("Logout");
        }
    }
}
