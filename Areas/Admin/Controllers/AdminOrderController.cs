using HuynhMinhTri_2122110256.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HuynhMinhTri_2122110256.Areas.Admin.Controllers
{
    public class AdminOrderController : Controller
    {
        minhtriEntities objOrder = new minhtriEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var order = new List<Order>();
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
                
                order = objOrder.Orders.Where(p => p.Id == int.Parse(SearchString)).ToList();

            }
            else
            {
                order = objOrder.Orders.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            order = order.OrderByDescending(n => n.Id).Where(p => p.Status != 4).ToList();

            return View(order.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detail(int id) {
            HomeModel model = new HomeModel();
            var orderId = objOrder.Orders.FirstOrDefault(p => p.Id == id);

            var user = objOrder.Users.FirstOrDefault(p => p.id == orderId.UserId);

            model.User = user;

            var order = objOrder.OrderDetails.Where(p => p.OrderId == id).ToList();
            model.ListOrderDetail = order;

            var productList = new List<Product>();

            foreach (var item in order)
            {
                var products = objOrder.Products.Where(p => p.Id == item.ProductId).ToList();
                productList.AddRange(products);
            }

            model.ListProduct = productList;
           

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var order = objOrder.Orders.Find(id);
            if (order == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
            }
            order.UpdateOnUtc = DateTime.Now;
            order.Status = status;
            objOrder.SaveChanges();
            return Json(new { success = true, message = "Tình trạng đơn hàng được cập nhật thành công" });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var order = objOrder.Orders.Where(p => p.Id == id).FirstOrDefault();
            order.Status = 4;
            objOrder.Entry(order).State = EntityState.Modified;
            objOrder.SaveChanges();
            return View(order);
        }

        [HttpPost]
        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Trash(string currentFilter, string SearchString, int? page)
        {
            var order = new List<Order>();
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
                order = objOrder.Orders.Where(p => p.Name.Contains(SearchString)).ToList();
            }
            else
            {
                order = objOrder.Orders.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            order = order.OrderByDescending(n => n.Id).Where(p => p.Status == 4).ToList();

            return View(order.ToPagedList(pageNumber, pageSize));
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
            var order = objOrder.Orders.Where(p => p.Id == id).FirstOrDefault();

            order.Status = 1;
            objOrder.Entry(order).State = EntityState.Modified;
            objOrder.SaveChanges();
            return View(order);
        }

        //Xóa vĩnh viễn
        [HttpGet]
        public ActionResult Destroy(int id)
        {
            var order = objOrder.Orders.Where(p => p.Id == id).FirstOrDefault();

            objOrder.Orders.Remove(order);
            objOrder.SaveChanges();
            return View(order);
        }
        [HttpPost]
        public ActionResult Destroy()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Order(int ?id)
        {

            HomeModel model = new HomeModel();
            model.User = objOrder.Users.FirstOrDefault(p => p.id == id);
            IQueryable<Order> orderQuery = objOrder.Orders.Where(p => p.UserId == id);
            if(id > 0)
            {
                model.ListOrder = orderQuery.OrderByDescending(p => p.Id).ToList();
            }
            else
            {
                ViewBag.Message = "Không có khách hàng này trong danh sách";
            }
           
            return View(model);
        }

        public ActionResult OrderDetail(int id)
        {
            HomeModel model = new HomeModel();

            var order = objOrder.OrderDetails.Where(p => p.OrderId == id).ToList();
            model.ListOrderDetail = order;

            var productList = new List<Product>();

            foreach (var item in order)
            {

                var products = objOrder.Products.Where(p => p.Id == item.ProductId).ToList();
                productList.AddRange(products);
            }

            model.ListProduct = productList;

            return View(model);
        }

    }
}