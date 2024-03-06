using Microsoft.AspNetCore.Mvc;

namespace SV20T1020639.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = " Trần Nhơn Viết",
                Birthday = DateTime.Now,
                Salary = 500.25m
            };
            return View(model);
        }

        public IActionResult Save(Models.Person model)
        {
            return Json(model);
        }
    }
}
