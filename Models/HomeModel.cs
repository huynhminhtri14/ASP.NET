using HuynhMinhTri_2122110256.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;

namespace HuynhMinhTri_2122110256.Models
{
    public class HomeModel
    {
        public List<CartModel> Cart { get; set; }
        //public Post Post { get; set; }
        //public List<Post> ListPost { get; set; }
        

        public List<Order> ListOrder { get; set; }
        public List<OrderDetail> ListOrderDetail { get; set; }
        public User User { get; set; }
        public List<User> ListUser { get; set; }
        public Product Product { get; set; }
        public List<Product> ListProduct { get; set; }
        public List<Category> ListCategory { get; set; }

        public List<Brand> ListBrand { get; set; }
        //public List<Banner> ListBanner { get; set; }

        public int countOrder { get; set; }
        public int countUser { get; set; }
    }
}