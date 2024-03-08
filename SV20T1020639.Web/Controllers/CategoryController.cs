using Microsoft.AspNetCore.Mvc;
using SV20T1020639.BusinessLayers;
using SV20T1020639.DomainModels;
using SV20T1020639.Web.Models;

namespace SV20T1020639.Web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_TITLE = " nhập loại hàng mới";

        const string CATEGORY_SEARCH = "category_search";//session dùng để lưu lại điều kiện tìm kiếm
        public IActionResult Index()
        {
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEGORY_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CategorySearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data

            };

            ApplicationContext.SetSessionData(CATEGORY_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            var model = new Category()

            {
                CategoryID = 0,
            };

            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "cập nhật thông tin loại hàng";
            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        public IActionResult Save(Category model) // nhieu qua nên moi xai model
        {
            if (string.IsNullOrWhiteSpace(model.CategoryName))
                ModelState.AddModelError("CategoryName", "Tên loại hàng được để trống"); //tên lỗi + thông báo lỗi
            if (string.IsNullOrWhiteSpace(model.Description))
                ModelState.AddModelError("Description", "vui lòng nhập thông tin chi tiết cho loại hàng");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Bổ sung loại hàng" : "cập nhật thông tin loại hàng";
                return View("Edit", model);
            }
            if (model.CategoryID == 0)
            {
                int id = CommonDataService.AddCategory(model);
                if (id < 0)
                {
                    ModelState.AddModelError("CategoryName", "Tên loại hàng bị trùng");
                    ViewBag.Title = CREATE_TITLE;
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateCategory(model);
                ModelState.AddModelError("Error", "Không cập nhật được loại hàng. Có thể tên loại hàng bị trùng");
                return View("Edit", model);
            }
            return RedirectToAction("Index");
        } //Ctrl + R + R , refactor thay đổi đồng bộ
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}
