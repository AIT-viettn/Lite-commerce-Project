using Microsoft.AspNetCore.Mvc;
using SV20T1020639.BusinessLayers;
using SV20T1020639.DomainModels;

namespace SV20T1020639.Web.Controllers
{
    public class SupplierController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_TITLE = " nhập nhà cung cấp mới";

        public IActionResult Index(int page = 1, string searchValue = "")
        {


            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.SupplierSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            return View(model); // dữ liệu truyền cho View có kiểu dữ liệu Model.SupplierSearchResult
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            var model = new Supplier()

            {
                SupplierID = 0,
            };

            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "cập nhật thông tin nhà cung cấp";
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Supplier model) // nhieu qua nên moi xai model
        {
            if (string.IsNullOrWhiteSpace(model.SupplierName))
                ModelState.AddModelError("SupplierName", "Tên nhà cung cấp được để trống"); //tên lỗi + thông báo lỗi
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email không được để trống");
            if (string.IsNullOrWhiteSpace(model.Provice))
                ModelState.AddModelError("Provice", "Vui lòng chọn tỉnh/thành");
            if (model.SupplierID == 0)
            {
                int id = CommonDataService.AddSupplier(model);
            }
            else
            {
                bool result = CommonDataService.UpdateSupplier(model);
            }
            return RedirectToAction("Index");
        } //Ctrl + R + R , refactor thay đổi đồng bộ
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}
