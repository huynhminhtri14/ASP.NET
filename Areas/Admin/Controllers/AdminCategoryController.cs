using HuynhMinhTri_2122110256.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace HuynhMinhTri_2122110256.Areas.Admin.Controllers
{
    public class AdminCategoryController : Controller
    {
        minhtriEntities dbObj = new minhtriEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var category = new List<Category>();
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
                category = dbObj.Categories.Where(p => p.categoryName.Contains(SearchString)).ToList();
            }
            else
            {
                category = dbObj.Categories.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            category = category.OrderByDescending(n => n.category_id).Where(p => p.deleted == false).ToList();

            return View(category.ToPagedList(pageNumber, pageSize));
        }


        //Tạo
        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null)
                {
                    string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName).ToLower();

                    if (!permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageUpload", "Chỉ cho phép tải lên các định dạng hình ảnh: .jpg, .jpeg, .png, .webp");
                        return View(objCategory);
                    }

                    const int maxSize = 5 * 1024 * 1024; // 5MB
                    if (objCategory.ImageUpload.ContentLength > maxSize)
                    {
                        ModelState.AddModelError("ImageUpload", "Kích thước tệp không được vượt quá 5MB");
                        return View(objCategory);
                    }

                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objCategory.image = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/category/"), fileName));
                }
                if (objCategory.description.Length > 500)
                {
                    ModelState.AddModelError("Description", "Mô tả không được vượt quá 500 ký tự.");
                }
                objCategory.createdOnUtc = DateTime.Now;
                objCategory.deleted = false;
                dbObj.Categories.Add(objCategory);
                dbObj.SaveChanges();
                return RedirectToAction("Index", "AdminCategory");
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
            var objCategory = dbObj.Categories.Where(p => p.category_id == id).FirstOrDefault();
            return View(objCategory);
        }

        //Xóa khỏi danh sách
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = dbObj.Categories.Where(p => p.category_id == id).FirstOrDefault();
            objCategory.deleted = true;
            dbObj.Entry(objCategory).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objCategory);
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
            var objCategory = dbObj.Categories.Where(p => p.category_id == id).FirstOrDefault();

            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Edit(Category objCategory, string ExistingImagePath)
        {
            if (ModelState.IsValid)
            {
                if (objCategory.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objCategory.image = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/category/"), fileName));
                    
                }
                else
                {
                    objCategory.image = ExistingImagePath;
                }
                objCategory.updateOnUtc = DateTime.Now;
                objCategory.deleted = false;
                dbObj.Entry(objCategory).State = EntityState.Modified;
                dbObj.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(objCategory);
        }


        //Xóa vĩnh viễn
        [HttpGet]
        public ActionResult Destroy(int id)
        {
            var objCat = dbObj.Categories.Where(p => p.category_id == id).FirstOrDefault();

            dbObj.Categories.Remove(objCat);
            dbObj.SaveChanges();
            return View(objCat);
        }
        [HttpPost]
        public ActionResult Destroy()
        {
            return RedirectToAction("Trash");
        }


        //Thùng rác
        [HttpGet]
        public ActionResult Trash(string currentFilter, string SearchString, int? page)
        {
            var category = new List<Category>();
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
                category = dbObj.Categories.Where(p => p.categoryName.Contains(SearchString)).ToList();
            }
            else
            {
                category = dbObj.Categories.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            category = category.OrderByDescending(n => n.category_id).Where(p => p.deleted == true).ToList();

            return View(category.ToPagedList(pageNumber, pageSize));
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
            var objCat = dbObj.Categories.Where(p => p.category_id == id).FirstOrDefault();

            objCat.deleted = false;
            dbObj.Entry(objCat).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objCat);
        }
    }
}