using Microsoft.AspNetCore.Mvc;
using SV20T1020639.BusinessLayers;
using SV20T1020639.DomainModels;

namespace SV20T1020639.Web.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {


            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            var model = new Models.EmployeeSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
            return View(model); // dữ liệu truyền cho View có kiểu dữ liệu Model.EmployeeSearchResult
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            var model = new Employee()

            {
                EmployeeID = 0,
                Photo = "nophoto.png",
                BirthDate = new DateTime(1990,1,1)
            };

            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "cập nhật thông tin nhân viên";
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            if (string.IsNullOrWhiteSpace(model.Photo))
                model.Photo = "nophoto.png";

            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Employee model, string birthDateInput = "", IFormFile? uploadPhoto = null) // nhieu qua nên moi xai model
        {
            //Xử lý ngày sinh
            DateTime? d = birthDateInput.ToDateTime();
            if (d.HasValue)
                model.BirthDate = d.Value;

            //Xử lý ảnh upload: Nếu có ảnh được upload thì lưu ảnh lên server, gán tên file ảnh đã lưu cho model.Photo
            if (uploadPhoto != null)
            {
                //Tên file sẽ lưu trên server
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu trên server
                                                                                  //Đường dẫn đến file sẽ lưu trên server 
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\employees", fileName);

                //Lưu file lên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                //Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }
            if (model.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(model);
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(model);
            }
            return RedirectToAction("Index");
        } //Ctrl + R + R , refactor thay đổi đồng bộ
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}
