using HuynhMinhTri_2122110256.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace HuynhMinhTri_2122110256.Controllers
{
    public class OrderController : Controller
    {
        minhtriEntities dbOrder = new minhtriEntities();
        public ActionResult Index(int id)
        {

            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            HomeModel model = new HomeModel();
            IQueryable<Order> orderQuery = dbOrder.Orders.Where(p => p.UserId == id);
            model.ListOrder = orderQuery.OrderByDescending(p => p.Id).ToList();
            return View(model);
        }

        public ActionResult OrderDetail(int id)
        {
            HomeModel model = new HomeModel();

            var order = dbOrder.OrderDetails.Where(p => p.OrderId == id).ToList();
            model.ListOrderDetail = order;

            var productList = new List<Product>();

            foreach (var item in order)
            {

                var products = dbOrder.Products.Where(p => p.Id == item.ProductId).ToList();
                productList.AddRange(products);
            }

            model.ListProduct = productList;

            return View(model);
        }

    }
}