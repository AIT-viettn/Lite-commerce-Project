using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using SV20T1020639.BusinessLayers;
using SV20T1020639.DomainModels;
using SV20T1020639.Web.Models;
using System;

namespace SV20T1020639.Web.Controllers
{
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_TITLE = " nhập mặt hàng mới";
        const string PRODUCT_SEARCH = "product_search";//session dùng để lưu lại điều kiện tìm kiếm
        public ActionResult Index()
        {
            Models.ProductSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            {
                if (input == null)
                {
                    input = new ProductSearchInput()
                    {
                        Page = 1,
                        PageSize = PAGE_SIZE,
                        SearchValue = "",
                        CategoryID = 0,
                        SupplierID = 0
                    };
                }
            };
            return View(input);
        }

        public ActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataServices.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID, input.SupplierID);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.IsEdit = false;
            ViewBag.Title = "Bổ sung thông tin mặt hàng";
            var model = new Product()

            {
                Photo = "nophoto1.jpg",
                ProductID = 0,
            };

            return View("Edit",model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.IsEdit = true;
            ViewBag.Title = "cập nhật thông tin mặt hàng ";
            var product = ProductDataServices.GetProduct(id);
            List<ProductAttribute> productAttributes = ProductDataServices.ListAttributes(id);
            List<ProductPhoto> productPhotos = ProductDataServices.ListPhotos(id);
            if (product == null || productAttributes == null || productPhotos == null)
                return RedirectToAction("Index");
            if (string.IsNullOrWhiteSpace(product.Photo))
                product.Photo = "nophoto1.jpg";
            Models.ProductTotal model = new Models.ProductTotal()
            {
                Product = product,
                ProductAttributes = productAttributes,
                ProductPhotos = productPhotos
            };
            return View(model);
        }
        public IActionResult Save(Product model, IFormFile? uploadPhoto = null) // nhieu qua nên moi xai model
        {
            if (string.IsNullOrWhiteSpace(model.ProductName))
                ModelState.AddModelError("ProductName", "Tên cùa mặt hàng không được để trống"); //tên lỗi + thông báo lỗi
            if (model.CategoryID == 0)
                ModelState.AddModelError("CategoryID", "vui lòng chọn loại hàng");
            if (model.SupplierID == 0)
                ModelState.AddModelError("SupplierID", "vui lòng chọn nhà cung cấp");
            if (string.IsNullOrWhiteSpace(model.Unit))
            {
                ModelState.AddModelError("Unit", "Đơn vị tính của mặt hàng không được để trống");
            }
            if (model.Price == 0)
            {
                ModelState.AddModelError("Price", "Giá của mặt hàng không được để trống");
            }
            if (model.ProductID == 0 && model.Photo == null)
            {
                ModelState.AddModelError("Photo", "Vui lòng thêm ảnh");
            }

            if (uploadPhoto != null)
            {
                //Tên file sẽ lưu trên server
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu trên server
                                                                                  //Đường dẫn đến file sẽ lưu trên server 
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products", fileName);

                //Lưu file lên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                //Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Bổ sung mặt hàng" : "cập nhật thông tin mặt hàng";
                return View("Edit", model);
            }
            if (model.ProductID == 0)
            {
                int id = ProductDataServices.AddProduct(model);
                if (id < 0)
                {
                    ModelState.AddModelError("ProductName", "Tên mặt hàng bị trùng");
                    ViewBag.Title = CREATE_TITLE;
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = ProductDataServices.UpdateProduct(model);
                ModelState.AddModelError("Error", "Không cập nhật được mặt hàng. Có thể tên mặt hàng bị trùng");
                return View("Edit", model);
            }
            return RedirectToAction("Index");
        } //Ctrl + R + R , refactor thay đổi đồng bộ
        public IActionResult Delete(int id = 0)
        {

            if (Request.Method == "POST")
            {
                bool result = ProductDataServices.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataServices.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        public IActionResult Photo(int id = 0 ,string method = "", int photoId = 0)
        {
            ProductPhoto model = null;
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    model = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    if (photoId < 0)
                    {
                        return RedirectToAction("Index");
                    }
                    model = ProductDataServices.GetPhoto(photoId);
                    if (model == null)
                    {
                        return RedirectToAction("index");
                    }
                    return View(model);
                case "delete":
                    ProductDataServices.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id }); 
                default:
                    return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public ActionResult SavePhoto(ProductPhoto model, IFormFile? uploadPhoto = null)
        {
            if (string.IsNullOrWhiteSpace(model.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị hình ảnh không được để trống");
            }
            else if (model.DisplayOrder < 1)
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị hình ảnh phải là một số tự nhiên dương");
            }
            List<ProductPhoto> productPhotos = ProductDataServices.ListPhotos(model.ProductID);
            bool isUsedDisplayOrder = false;
            foreach (ProductPhoto item in productPhotos)
            {
                if (item.DisplayOrder == model.DisplayOrder && model.PhotoID != item.PhotoID)
                {
                    isUsedDisplayOrder = true;
                    break;
                }
            }
            if (isUsedDisplayOrder)
            {
                ModelState.AddModelError("DisplayOrder",
                    $"Thứ tự hiển thị {model.DisplayOrder} của hình ảnh đã được sử dụng trước đó");
            }

            model.Description = model.Description ?? "";
            model.IsHidden = Convert.ToBoolean(model.IsHidden.ToString());
            // xử lý nghiệp vụ upload file
            if (uploadPhoto != null)
            {
                //Tên file sẽ lưu trên server
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu trên server
                                                                                  //Đường dẫn đến file sẽ lưu trên server 
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products", fileName);

                //Lưu file lên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                //Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.PhotoID == 0 ? "Bổ sung ảnh" : "Thay đổi ảnh";
                return View("Photo", model);
            }

            // thực hiện thêm hoặc cập nhật
            if (model.PhotoID == 0)
            {
                ProductDataServices.AddPhoto(model);
            }
            else
            {
                ProductDataServices.UpdatePhoto(model);
            }
            return RedirectToAction("Edit", model);
        }
        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            ProductAttribute model = null;
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    model = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id,
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    if (attributeId < 0)
                    {
                        return RedirectToAction("Index");
                    }
                    model = ProductDataServices.GetAttribute(attributeId);
                    if (model == null)
                    {
                        return RedirectToAction("Index");
                    }
                    return View(model);
                case "delete":
                    ProductDataServices.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });  
                default:
                    return RedirectToAction("Index");
            }

        }
        [HttpPost]
        public ActionResult SaveAttribute(ProductAttribute model)
        {
            // kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.AttributeName))
            {
                ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.AttributeValue))
            {
                ModelState.AddModelError("AttributeValue", "Giá trị thuộc tính không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị thuộc tính không được để trống");
            }
            else if (model.DisplayOrder < 1)
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị thuộc tính phải là một số tự nhiên dương");
            }
            List<ProductAttribute> productAttributes = ProductDataServices.ListAttributes(model.ProductID);
            bool isUsedDisplayOrder = false;
            foreach (ProductAttribute item in productAttributes)
            {
                if (item.DisplayOrder == model.DisplayOrder && model.AttributeID != item.AttributeID)
                {
                    isUsedDisplayOrder = true;
                    break;
                }
            }
            if (isUsedDisplayOrder)
            {
                ModelState.AddModelError("DisplayOrder",
                        $"Thứ tự hiển thị {model.DisplayOrder} của thuộc tính đã được sử dụng trước đó");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.AttributeID == 0 ? "Bổ sung thuộc tính" : "Thay đổi thuộc tính";
                return View("Attribute", model);
            }

            // thực hiện thêm hoặc cập nhật
            if (model.AttributeID == 0)
            {
                ProductDataServices.AddAttribute(model);
            }
            else
            {
                ProductDataServices.UpdateAttribute(model);
            }
            return RedirectToAction("Edit" , model.ProductID);
        }
    }
}
