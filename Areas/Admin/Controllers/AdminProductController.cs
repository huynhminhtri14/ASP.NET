using HuynhMinhTri_2122110256.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HuynhMinhTri_2122110256.Areas.Admin.Controllers
{
    public class AdminProductController : Controller
    {
        minhtriEntities dbObj = new minhtriEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var product = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;

            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                product = dbObj.Products.Where(p => p.Name.Contains(SearchString)).ToList();
            }
            else
            {
                product = dbObj.Products.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            product = product.OrderByDescending(n => n.Id).Where(p => p.Deleted == false).ToList();

            return View(product.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            try
            {
                if (objProduct.ImageUpload != null)
                {
                    string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName).ToLower();

                    if (!permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageUpload", "Chỉ cho phép tải lên các định dạng hình ảnh: .jpg, .jpeg, .png, .webp");
                        return View(objProduct);
                    }

                    const int maxSize = 5 * 1024 * 1024; // 5MB
                    if (objProduct.ImageUpload.ContentLength > maxSize)
                    {
                        ModelState.AddModelError("ImageUpload", "Kích thước tệp không được vượt quá 5MB");
                        return View(objProduct);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objProduct.Image = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product/"), fileName));
                }
                if (objProduct.Description.Length > 500) // Ví dụ: giới hạn mô tả là 500 ký tự
                {
                    ModelState.AddModelError("Description", "Mô tả không được vượt quá 500 ký tự.");
                }
                objProduct.CreatedOnUtc = DateTime.Now;
                objProduct.Deleted = false;
                dbObj.Products.Add(objProduct);
                dbObj.SaveChanges();

                return RedirectToAction("Index", "AdminProduct");
            }
            catch (Exception ex)
            {
                // Ghi lỗi vào log hoặc hiển thị thông báo lỗi
                ModelState.AddModelError("", "Có lỗi xảy ra khi xử lý yêu cầu: " + ex.Message);
            }

            return View(objProduct);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var objProduct = dbObj.Products.Where(p => p.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = dbObj.Products.Where(p => p.Id == id).FirstOrDefault();
            objProduct.Deleted = true;
            dbObj.Entry(objProduct).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objProduct)
        {
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = dbObj.Products.Where(p => p.Id == id).FirstOrDefault();

            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(Product objProduct, string ExistingImagePath)
        {
            if (ModelState.IsValid)
            {
                if (objProduct.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objProduct.Image = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product/"), fileName));
                }
                else
                {
                    objProduct.Image = ExistingImagePath;
                }
                objProduct.UpdateOnUtc = DateTime.Now;
                objProduct.Deleted = false;
                dbObj.Entry(objProduct).State = EntityState.Modified;
                dbObj.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(objProduct);
        }
        //Xóa vĩnh viễn
        [HttpGet]
        public ActionResult Destroy(int id)
        {
            var objProduct = dbObj.Products.Where(p => p.Id == id).FirstOrDefault();

            dbObj.Products.Remove(objProduct);
            dbObj.SaveChanges();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Destroy()
        {
            return RedirectToAction("Index");
        }


        //Thùng rác
        [HttpGet]
        public ActionResult Trash(string currentFilter, string SearchString, int? page)
        {
            var product = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;

            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                product = dbObj.Products.Where(p => p.Name.Contains(SearchString)).ToList();
            }
            else
            {
                product = dbObj.Products.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            product = product.OrderByDescending(n => n.Id).Where(p => p.Deleted == true).ToList();

            return View(product.ToPagedList(pageNumber, pageSize));
        }

        //Khôi phục
        [HttpPost]
        public ActionResult Restore()
        {
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Restore(int id)
        {
            var objProduct = dbObj.Products.Where(p => p.Id == id).FirstOrDefault();

            objProduct.Deleted = false;
            dbObj.Entry(objProduct).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objProduct);
        }
    }
}
