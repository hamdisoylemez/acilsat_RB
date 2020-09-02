﻿using acilsat_RB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace acilsat_RB.Controllers
{
    public class ProductsController : Controller
    {
        acilsatDB db = new acilsatDB();
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductDetail(int id)
        {
            Products product = db.Products.Find(id);
            Users user = db.Users.Find(product.userId);
            string nameSurname = user.name + " " + user.surName;
            ViewData["categoryNo"] = product.categoryNo;
            ViewData["nameSurname"] = nameSurname;
            ViewData["userCity"] = user.userCity;
            ViewData["userPhone"] = user.userPhone;
            return View(product);
        }
        public ActionResult ProductAdd([Bind(Include = "productName,categoryNo,productPrice,productDescription")] Products product)
        {
            Random rnd = new Random();
            if (ModelState.IsValid)
            {
                int counter = 0;
                product.userId = Convert.ToInt32(TempData["userId"]);
                int rndProductNo = rnd.Next(1000000, 9999999);
                while (counter < 100)
                {
                    var productCheck = db.Products.Where(x => x.productNo == rndProductNo).SingleOrDefault();
                    if (productCheck != null)
                    {
                        rndProductNo = rnd.Next(1000000, 999999);
                    }
                    else
                    {
                        product.productNo = rndProductNo;
                        break;
                    }
                    counter++;
                }
                db.Products.Add(product);
                db.SaveChanges();
                ModelState.Clear();
            }
            return RedirectToAction("UserProfile","Users");
        }
    }
}