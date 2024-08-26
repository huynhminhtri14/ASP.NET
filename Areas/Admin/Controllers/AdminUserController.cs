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
    public class AdminUserController : Controller
    {
        minhtriEntities dbObj = new minhtriEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var user = new List<User>();
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
                user = dbObj.Users.Where(p => p.phone.Contains(SearchString)).ToList();
            }
            else
            {
                user = dbObj.Users.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            user = user.OrderByDescending(n => n.id).Where(p => p.deleted == false).ToList();

            return View(user.ToPagedList(pageNumber, pageSize));
        }


        //Tạo
        [HttpPost]
        public ActionResult Create(User objUser)
        {
            try
            {
                objUser.createdAt = DateTime.Now;
                objUser.deleted = false;
                dbObj.Users.Add(objUser);
                dbObj.SaveChanges();
                return RedirectToAction("Index", "AdminUser");
            }
            catch (Exception)
            {
                Console.WriteLine("Lỗi thêm người dùng!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //Chi tiết, xem đơn hàng
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var objUser = dbObj.Users.Where(p => p.id == id).FirstOrDefault();
            return View(objUser);
        }

        //Xóa khỏi danh sách
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objUser = dbObj.Users.Where(p => p.id == id).FirstOrDefault();
            objUser.deleted = true;
            dbObj.Entry(objUser).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objUser);
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
            var objUser = dbObj.Users.Where(p => p.id == id).FirstOrDefault();

            return View(objUser);
        }

        [HttpPost]
        public ActionResult Edit(User objUser)
        {
            try
            {
                objUser.role = objUser.role;
                objUser.updateAt = DateTime.Now;
                objUser.deleted = false;
               
                dbObj.Entry(objUser).State = EntityState.Modified;
                dbObj.SaveChanges();
                return RedirectToAction("Index","AdminUser");                            
            }
            catch (Exception ex)
            {
              Console.WriteLine("Lỗi cập nhật người dùng: " + ex.Message);
            }

            return View(objUser);
        }

        //Cập nhật
        [HttpPost]
        public ActionResult UpdateRole(int id, int role)
        {
            var user = dbObj.Users.Find(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }
            user.updateAt = DateTime.Now;
            user.deleted = false;
            user.role = role;
            dbObj.SaveChanges();
            return Json(new { success = true, message = "Quyền hạn được cập nhật thành công" });
        }

        [HttpPost]
        public ActionResult UpdateShowOnAdminPage(int id, bool showOnAdminPage)
        {
            var user = dbObj.Users.Find(id); 
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }
            user.updateAt = DateTime.Now;
            user.deleted = false;
            user.showOnAdminPage = showOnAdminPage;
            dbObj.SaveChanges();
            return Json(new { success = true, message = "Cập nhật thành công." });
        }


        //Xóa vĩnh viễn
        [HttpGet]
        public ActionResult Destroy(int id)
        {
            var objUser = dbObj.Users.Where(p => p.id == id).FirstOrDefault();

            dbObj.Users.Remove(objUser);
            dbObj.SaveChanges();
            return View(objUser);
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
            var User = new List<User>();
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
                User = dbObj.Users.Where(p => p.phone.Contains(SearchString)).ToList();
            }
            else
            {
                User = dbObj.Users.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            User = User.OrderByDescending(n => n.id).Where(p => p.deleted == true).ToList();

            return View(User.ToPagedList(pageNumber, pageSize));
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
            var objUser = dbObj.Users.Where(p => p.id == id).FirstOrDefault();

            objUser.deleted = false;
            dbObj.Entry(objUser).State = EntityState.Modified;
            dbObj.SaveChanges();
            return View(objUser);
        }
    }
}