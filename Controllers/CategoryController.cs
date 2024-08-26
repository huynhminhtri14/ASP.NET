
using HuynhMinhTri_2122110256.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HuynhMinhTri_2122110256.Controllers
{
    public class CategoryController : Controller
    {
       
        // GET: Category
        minhtriEntities objCategoryEntities = new minhtriEntities();
        public ActionResult Category()
        {
            HomeModel homeModel = new HomeModel();
            homeModel.ListCategory = objCategoryEntities.Categories.ToList();
            return View(homeModel);
        }

    }

}