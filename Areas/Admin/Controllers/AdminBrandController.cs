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
    public class AdminBrandController : Controller
    {
        minhtriEntities dbObj = new minhtriEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var brand = new List<Brand>();
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
                brand = dbObj.Brands.Where(p => p.brandName.Contains(SearchString)).ToList();
            }
            else
            {
                brand = dbObj.Brands.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            brand = brand.OrderByDescending(n => n.id).Where(p => p.deleted == false).ToList();

            return View(brand.ToPagedList(pageNumber, pageSize));
        }


        //Tạo
        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {
            try
            {
                if (objBrand.ImageUpload != null)
                {
                    string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName).ToLower();

                    if (!permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageUpload", "Chỉ cho phép tải lên các định dạng hình ảnh: .jpg, .jpeg, .png, .webp");
                        return View(objBrand);
                    }

                    const int maxSize = 5 * 1024 * 1024; // 5MB
                    if (objBrand.ImageUpload.ContentLength > maxSize)
                    {
                        ModelState.AddModelError("ImageUpload", "Kích thước tệp không được vượt quá 5MB");
                        return View(objBrand);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBrand.image = fileName;
                    objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/brand/"), fileName));
                }
                if (objBrand.description.Length > 500) 
                {
                    ModelState.AddModelError("Description", "Mô tả không được vượt quá 500 ký tự.");
                }
                objBrand.createdOnUtc = DateTime.Now;
                objBrand.deleted = false;
                dbObj.Brands.Add(objBrand);
                dbObj.SaveChanges();
                return RedirectToAction("Index", "AdminBrand");
            }
            catch (Exception)
            {
                Console.WriteLine("Lỗi upload!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //Chi tiết
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var objBrand = dbObj.Brands.Where(p => p.id == id).FirstOrDefault();
            return View(objBrand);
        }

        //Xóa khỏi danh sách
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = dbObj.Brands.Where(p => p.id == id).FirstOrDefault();
            objBrand.deleted = true;
            dbObj.Entry(objBrand).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }

        //Chỉnh sửa
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBrand = dbObj.Brands.Where(p => p.id == id).FirstOrDefault();

            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Edit(Brand objBrand, string ExistingImagePath)
        {
            if (ModelState.IsValid)
            {
                if (objBrand.ImageUpload != null)
                {
                    string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName).ToLower();

                    if (!permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageUpload", "Chỉ cho phép tải lên các định dạng hình ảnh: .jpg, .jpeg, .png, .webp");
                        return View(objBrand);
                    }

                    const int maxSize = 5 * 1024 * 1024; // 5MB
                    if (objBrand.ImageUpload.ContentLength > maxSize)
                    {
                        ModelState.AddModelError("ImageUpload", "Kích thước tệp không được vượt quá 5MB");
                        return View(objBrand);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBrand.image = fileName;
                    objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/brand/"), fileName));
                }
                else
                {
                    objBrand.image = ExistingImagePath;
                }
                if (objBrand.description.Length > 500)
                {
                    ModelState.AddModelError("Description", "Mô tả không được vượt quá 500 ký tự.");
                }
                objBrand.updateOnUtc = DateTime.Now;
                objBrand.deleted = false;
                dbObj.Entry(objBrand).State = EntityState.Modified;
                dbObj.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(objBrand);
        }


        //Xóa vĩnh viễn
        [HttpGet]
        public ActionResult Destroy(int id)
        {
            var objBrand = dbObj.Brands.Where(p => p.id == id).FirstOrDefault();

            dbObj.Brands.Remove(objBrand);
            dbObj.SaveChanges();
            return View(objBrand);
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
            var brand = new List< Brand>();
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
                brand = dbObj.Brands.Where(p => p.brandName.Contains(SearchString)).ToList();
            }
            else
            {
                brand = dbObj.Brands.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            brand = brand.OrderByDescending(n => n.id).Where(p => p.deleted == true).ToList();

            return View(brand.ToPagedList(pageNumber, pageSize));
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
            var objBrand = dbObj.Brands.Where(p => p.id == id).FirstOrDefault();

            objBrand.deleted = false;
            dbObj.Entry(objBrand).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objBrand);
        }
    }
}