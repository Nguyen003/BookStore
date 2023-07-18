using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using PagedList;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private DbBookStore db = new DbBookStore();
        // GET: Book
        public ActionResult Index(int? size, int? page, string searchString, string sortProperty, string sortOrder, int categoryID = 0)
        {

            //Search
            ViewBag.Keyword = searchString;                     // 1. Lưu tư khóa tìm kiếm
            var books = db.Books.Include(b => b.Author).Include(b => b.Category);           //2.Tạo câu truy vấn kết 3 bảng Book, Author, Category
            if (!String.IsNullOrEmpty(searchString))
                books = books.Where(b => b.Title.Contains(searchString));          //3. Tìm kiếm theo searchString
            if (categoryID != 0)
            {
                books = books.Where(c => c.CategoryID == categoryID);       //4. Tìm kiếm theo CategoryID
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName"); // danh sách Category

            //Sắp Xếp
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;

                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            // Phân trang
            if (searchString != null) page = 1;
            var pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(books.ToPagedList(pageNumber, pageSize));


        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(Book book, HttpPostedFileBase Images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Images.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(Images.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Images/bookimages"), _FileName);
                        Images.SaveAs(_path);
                        book.Images = _FileName;
                    }
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }

            }

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            return View(book);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName");
            return View(book);
        }

        // POST: Book/Edit/5
        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileBase Images, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Images != null)
                    {
                        string _FileName = Path.GetFileName(Images.FileName);
                        string _path = Path.Combine(Server.MapPath("~/bookimages"), _FileName);
                        Images.SaveAs(_path);
                        book.Images = _FileName;
                        // get Path of old image for deleting it
                        _path = Path.Combine(Server.MapPath("~/bookimages"), form["oldimage"]);
                        if (System.IO.File.Exists(_path))
                            System.IO.File.Delete(_path);

                    }
                    else
                    {
                        book.Images = form["oldimage"];
                    }
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }

                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            return View(book);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Book book = db.Books.Find(id);
                db.Books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
