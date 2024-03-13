using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SV20T1020639.Web.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id = 0)
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult EditDetail(int id = 0,int productId = 0)
        {
            return View();
        }
        public IActionResult Shipping(int id = 0)
        {
            return View();
        }
    }
}
