using Microsoft.AspNetCore.Mvc;

namespace SV20T1020639.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {

            ViewBag.Title = "Bổ sung thông tin mặt hàng";
            ViewBag.IsEdit = false;
            return View("Edit");
        }
        public IActionResult Edit(string id)
        {

            ViewBag.Title = "cập nhật thông tin mặt hàng ";
            ViewBag.IsEdit = true;
            return View();
        }
        public IActionResult Delete(string id)
        {

            return View();
        }
        public IActionResult Photo(string id,string method, int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "cập nhật ảnh cho mặt hàng";
                    return View();
                case "delete":
                    //TODO: Xóa ảnh trực tiếp
                    return RedirectToAction("Edit",new {id = id});
                default:
                    return RedirectToAction("Index");
                   
            }
            
        }
        public IActionResult Attribute(string id, string method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "cập nhật thuộc tính cho mặt hàng";
                    return View();
                case "delete":
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");

            }

        }
    }
}
