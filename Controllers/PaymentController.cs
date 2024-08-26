using HuynhMinhTri_2122110256.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuynhMinhTri_2122110256.common;
using System.Configuration;
using System.Net.Mail;

namespace HuynhMinhTri_2122110256.Controllers
{
    public class PaymentController : Controller
    {
        minhtriEntities objminhtriEntities = new minhtriEntities();
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "User");

            }
            else
            {
                var listProduct = (List<CartModel>)Session["cart"];
                Order objOrder = new Order();
                objOrder.Name = "Đơn hàng - " + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnUtc = DateTime.Now;
                objOrder.Status = 1;
                objminhtriEntities.Orders.Add(objOrder);
                try
                {
                    // Thực hiện lưu thay đổi
                    objminhtriEntities.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }

                int orderId = objOrder.Id;
                List<OrderDetail> orderDetail = new List<OrderDetail>();
                foreach (var item in listProduct)
                {
                    OrderDetail objOrderDetail = new OrderDetail();
                    objOrderDetail.Quantity = item.Quantity;
                    objOrderDetail.OrderId = orderId;
                    objOrderDetail.ProductId = item.Product.Id;
                    orderDetail.Add(objOrderDetail);
                }
                objminhtriEntities.OrderDetails.AddRange(orderDetail);
                try
                {
                    // Thực hiện lưu thay đổi
                    objminhtriEntities.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
                Session["count"] = 0;
                Session["cart"] = null;
                listProduct.Clear();
                int userId = int.Parse(Session["idUser"].ToString());
                var user = objminhtriEntities.Users.FirstOrDefault(p => p.id == userId);

               
            }
            return View();

        }
    }
}