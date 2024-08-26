using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HuynhMinhTri_2122110256
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["ActiveUsers"] = 0;  // Số lượng người dùng đang truy cập
            Application["TotalVisitors"] = 0; // Tổng số lượt truy cập

        }
        protected void Session_Start()
        {
            // Tăng số lượng người dùng đang truy cập
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] + 1;

            // Tăng tổng số lượt truy cập
            Application["TotalVisitors"] = (int)Application["TotalVisitors"] + 1;
        }

        protected void Session_End()
        {
            // Giảm số lượng người dùng đang truy cập
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] - 1;
        }
    }
}
