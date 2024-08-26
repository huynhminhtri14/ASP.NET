using HuynhMinhTri_2122110256.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HuynhMinhTri_2122110256.Controllers
{
    public class ProductController : Controller
    {
        minhtriEntities minhtriEntities = new minhtriEntities();

        public ActionResult Index(String searchString, int page = 1, int pageSize = 5)
        {
            HomeModel model = new HomeModel();
            IQueryable<Product> productsQuery = minhtriEntities.Products;

            if (searchString != null)
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchString));

            }

            model.ListProduct = productsQuery
                .OrderBy(p => p.Id)  // Ensure consistent ordering
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            model.ListCategory = minhtriEntities.Categories.ToList();
            model.ListBrand = minhtriEntities.Brands.ToList();

            foreach (var brand in model.ListBrand)
            {
                brand.ProductCount = productsQuery.Count(p => p.Brand_id == brand.id);
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(productsQuery.Count() / (double)pageSize);

            return View(model);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = minhtriEntities.Products.FirstOrDefault(p => p.Id == id);
            HomeModel model = new HomeModel();
            model.Product = product;


            return View(model);
        }
        public ActionResult Product_list_grid()
        {
            HomeModel model = new HomeModel();
            model.ListProduct = minhtriEntities.Products.ToList();
            model.ListCategory = minhtriEntities.Categories.ToList();
            model.ListBrand = minhtriEntities.Brands.ToList();

            return View(model);
        }
        public ActionResult Product_list_large()
        {
            HomeModel model = new HomeModel();
            model.ListProduct = minhtriEntities.Products.ToList();
            return View(model);
        }
        public ActionResult ProductByCategory(int id, int page = 1, int pageSize = 5)
        {
            HomeModel model = new HomeModel();
            model.ListCategory = minhtriEntities.Categories.ToList();
            model.ListBrand = minhtriEntities.Brands.ToList();
            var productsQuery = minhtriEntities.Products.Where(p => p.Category_id == id).ToList();
            model.ListProduct = productsQuery
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(productsQuery.Count() / (double)pageSize);

            return View(model);
        }

        public ActionResult ProductByBrand(int id, int page = 1, int pageSize = 5)
        {
            HomeModel model = new HomeModel();
            model.ListCategory = minhtriEntities.Categories.ToList();
            model.ListBrand = minhtriEntities.Brands.ToList();
            var productsQuery = minhtriEntities.Products.Where(p => p.Brand_id == id).ToList();
            model.ListProduct = productsQuery
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(productsQuery.Count() / (double)pageSize);

            return View(model);
        }

    }
}