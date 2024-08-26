using HuynhMinhTri_2122110256.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace HuynhMinhTri_2122110256.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        minhtriEntities obj = new minhtriEntities();

        public ActionResult Index()
        {
            HomeModel model = new HomeModel();
            model.ListOrder = obj.Orders
                                 .Where(p => DbFunctions.TruncateTime(p.CreatedOnUtc) == DbFunctions.TruncateTime(DateTime.Now))
                                 .ToList();

            model.countOrder = model.ListOrder.Count;

            if (model.countOrder == 0)
            {
                ViewBag.Error = "Không có đơn hàng mới hôm nay.";
            }
            int activeUsers = (int)HttpContext.Application["ActiveUsers"];

            int totalVisitors = (int)HttpContext.Application["TotalVisitors"];

            double bounceRate = activeUsers / totalVisitors;
            ViewBag.ActiveUsers = activeUsers;
            ViewBag.TotalVisitors = totalVisitors;

            model.ListUser = obj.Users
                                 .Where(p => DbFunctions.TruncateTime(p.createdAt) == DbFunctions.TruncateTime(DateTime.Now))
                                 .ToList();

            model.countUser = model.ListUser.Count;
            if (model.countUser == 0)
            {
                ViewBag.Error = "Không có đăng ký mới hôm nay.";
            }

            return View(model);
        }
    }
}
