using Microsoft.AspNetCore.Mvc;
using SV20T1020639.BusinessLayers;
using SV20T1020639.DomainModels;

namespace SV20T1020639.Web.Controllers
{
    public class ShipperController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_TITLE = " nhập đơn vị giao hàng mới";

        public IActionResult Index(int page = 1, string searchValue = "")
        {


            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.ShipperSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            return View(model); // dữ liệu truyền cho View có kiểu dữ liệu Model.ShipperSearchResult
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung thông tin đơn vị giao hàng";
            var model = new Shipper()

            {
                ShipperID = 0,
            };

            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "cập nhật thông tin đơn vị giao hàng ";
            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Shipper model) // nhieu qua nên moi xai model
        {
            if (string.IsNullOrWhiteSpace(model.ShipperName))
                ModelState.AddModelError("ShipperName", "Tên đơn vị không được để trống"); //tên lỗi + thông báo lỗi
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Số điện thoại không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? "Bổ sung đơn vị giao hàng" : "cập nhật thông tin đơn vị giao hàng";
                return View("Edit", model);
            }
            if (model.ShipperID == 0)
            {
                int id = CommonDataService.AddShipper(model);
                if (id < 0)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại bị trùng");
                    ViewBag.Title = CREATE_TITLE;
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateShipper(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được đơn vị giao hàng. Có thể số điện thoại bị trùng");
                    return View("Edit", model);
                }
            }
            return RedirectToAction("Index");
        } //Ctrl + R + R , refactor thay đổi đồng bộ
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}
