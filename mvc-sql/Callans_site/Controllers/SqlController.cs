using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Callans_site.Models;
using System.Linq.Dynamic;
using PagedList.Mvc;
using PagedList;

namespace Callans_site.Controllers
{
    public class SqlController : Controller
    {
        // GET: Sql
        public ActionResult Index()
        {
            return View();
        }

        static string[] ProductCols = new[] { "ProductID", "ProductName", "UnitPrice", "UnitsInStock", "UnitsOnOrder" }
            .Select(c => c.ToUpper()).ToArray();

        static string[] CategoryCols = new[] { "CategoryName", /* "DESCRIPTION", */ }
            .Select(c => c.ToUpper()).ToArray();

        static string[] SupplierCols = new[] { "CompanyName", "ContactName", "Country", }
            .Select(c => c.ToUpper()).ToArray();

        static string[] SortDirs = new[] { "ASC", "DESC", };

        //Get: SuppliersView?page=1&rowsPerPage=5&sortCol=OrderID&sortDir=ASC&categoryName=Beverages
        public ActionResult SuppliersView(int page = 1, int rowsPerPage = 3) {
            IPagedList<Callans_Suppliers_Model> res;
            int count;
            using(var nwd = new NorthwindEntities()) {
                var result = nwd.Suppliers
                    .OrderBy("Country")
                    .Select(s => new Callans_Suppliers_Model {
                        SupplierID = s.SupplierID,
                        CompanyName = s.CompanyName,
                        ContactName = s.ContactName,
                        ContactTitle = s.ContactTitle,
                        Address = s.Address,
                        City = s.City,
                        Region = s.Region,
                        PostalCode = s.PostalCode,
                        Country = s.Country,
                        Phone = s.Phone,
                        Fax = s.Fax,
                        HomePage = s.HomePage,
                    });
                res = result.ToPagedList<Callans_Suppliers_Model>(page, rowsPerPage);
                count = nwd.Suppliers.Count();
            }
            ViewBag.Count = count;
            return View(res);
        }


        // GET: CategoryView?page=1&rowsPerPage=5&sortCol=OrderID&sortDir=ASC&categoryName=Beverages
        public ActionResult CategoryView(int page = 1, int rowsPerPage = 5, string sortCol = "ProductID", string sortDir = "ASC", string categoryName = "Beverages") {
            string[] categoryNames = new[] { "Beverages", "Condiments", "Confections", "Dairy Products", "Grains/Cereals", "Meat/Poultry", "Produce", "Seafood" };
            sortCol = sortCol.ToUpper();
            if (ProductCols.Contains(sortCol)) ;
            else if (CategoryCols.Contains(sortCol)) sortCol = "Category." + sortCol;
            else if (SupplierCols.Contains(sortCol)) sortCol = "Supplier." + sortCol;
            else sortCol = "ProductID";

            sortDir = sortDir.ToUpper();
            if (SortDirs.Contains(sortDir)) ;
            else sortDir = "ASC";

            List<Callans_Products_Model> res;
            int count;

            using (var nwd = new NorthwindEntities()) {
                var result = nwd.Products.Where(n=>n.Category.CategoryName.ToLower().Equals(categoryName.ToLower()))
                    .OrderBy(sortCol + " " + sortDir + ", ProductID " + sortDir)
                    .Skip((page - 1) * rowsPerPage)
                    .Take(rowsPerPage)
                    .Select(p => new Callans_Products_Model {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        CategoryName = p.Category == null ? null : p.Category.CategoryName,
                        //Description = p.Category == null? null : p.Category.Description,  
                        CompanyName = p.Supplier == null ? null : p.Supplier.CompanyName,
                        ContactName = p.Supplier == null ? null : p.Supplier.ContactName,
                        Country = p.Supplier == null ? null : p.Supplier.Country,
                    });

                res = result.ToList();
                count = nwd.Products.Where(n => n.Category.CategoryName.ToLower().Equals(categoryName.ToLower())).Count();
            }

            ViewBag.sortCol = sortCol;
            ViewBag.rowsPerPage = rowsPerPage;
            ViewBag.count = count;
            ViewBag.categories = categoryNames.ToList<string>();

            return View(res);
        }

        // GET: ProductsView?page=1&rowsPerPage=10&sortCol=OrderID&sortDir=ASC
        public ActionResult ProductsView(int page = 1, int rowsPerPage = 10, string sortCol = "ProductID", string sortDir = "ASC") {
            sortCol = sortCol.ToUpper();
            if (ProductCols.Contains(sortCol)) ;
            else if (CategoryCols.Contains(sortCol)) sortCol = "Category." + sortCol;
            else if (SupplierCols.Contains(sortCol)) sortCol = "Supplier." + sortCol;
            else sortCol = "ProductID";

            sortDir = sortDir.ToUpper();
            if (SortDirs.Contains(sortDir)) ;
            else sortDir = "ASC";

            List<Callans_Products_Model> res;
            int count;
            string sql;

            using (var nwd = new NorthwindEntities()) {
                var result = nwd.Products
                    .Where(p => p.Discontinued==false)
                    .OrderBy(sortCol + " " + sortDir + ", ProductID " + sortDir)
                    .Skip((page - 1) * rowsPerPage)
                    .Take(rowsPerPage)
                    .Select(p => new Callans_Products_Model {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        UnitPrice = p.UnitPrice,
                        UnitsInStock = p.UnitsInStock,
                        UnitsOnOrder = p.UnitsOnOrder,
                        CategoryName = p.Category == null ? null : p.Category.CategoryName,
                        //Description = p.Category == null? null : p.Category.Description,  
                        CompanyName = p.Supplier == null ? null : p.Supplier.CompanyName,
                        ContactName = p.Supplier == null ? null : p.Supplier.ContactName,
                        Country = p.Supplier == null ? null : p.Supplier.Country,
                    });

                sql = result.ToString();
                res = result.ToList();
                count = nwd.Products.Where(p => p.Discontinued == false).Count();
            }

            ViewBag.sortCol = sortCol;
            ViewBag.rowsPerPage = rowsPerPage;
            ViewBag.count = count;
            ViewBag.sql = sql;
            return View(res);
        }
    }
}