using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;

namespace MVC5Course.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        private CustomerEntities db = new CustomerEntities();

        private 客戶聯絡人Repository contactRepository;
        private IEnumerable<SelectListItem> columnList;
        private IEnumerable<SelectListItem> orderList;

        public 客戶聯絡人Controller()
        {
            contactRepository = RepositoryHelper.Get客戶聯絡人Repository();
            columnList = new List<SelectListItem>
            {
                new SelectListItem { Value = null, Text = "請選擇" },
                new SelectListItem { Value = "職稱", Text = "職稱" },
                new SelectListItem { Value = "姓名", Text = "姓名" },
                new SelectListItem { Value = "Email", Text = "Email" },
                new SelectListItem { Value = "手機", Text = "手機" },
                new SelectListItem { Value = "電話", Text = "電話" },
                new SelectListItem { Value = "客戶名稱", Text = "客戶名稱" }
            };

            orderList = new List<SelectListItem>
            {
                new SelectListItem { Value = "asc", Text = "升序" },
                new SelectListItem { Value = "desc", Text = "降序" }
            };

        }

        // GET: 客戶聯絡人
        public ActionResult Index()
        {

            ViewBag.columnList = new SelectList(columnList, "Value", "Text");
            ViewBag.orderList = new SelectList(orderList, "Value", "Text");
            var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料);
            return View(客戶聯絡人.Where(x => !x.是否已刪除).ToList());
        }

        [HttpPost]
        public ActionResult Index(string searchText, string column, string order)
        {
            ViewBag.columnList = new SelectList(columnList, "Value", "Text");
            ViewBag.orderList = new SelectList(orderList, "Value", "Text");
            var query = db.客戶聯絡人.Include(客 => 客.客戶資料).Where(x => x.職稱.Contains(searchText) && !x.是否已刪除);

            if (!String.IsNullOrEmpty(column))
            {
                if (order == "asc")
                {
                    switch (column)
                    {
                        case "職稱":
                            query = query.OrderBy(x => x.職稱);
                            break;
                        case "姓名":
                            query = query.OrderBy(x => x.姓名);
                            break;
                        case "Email":
                            query = query.OrderBy(x => x.Email);
                            break;
                        case "手機":
                            query = query.OrderBy(x => x.手機);
                            break;
                        case "電話":
                            query = query.OrderBy(x => x.電話);
                            break;
                        case "客戶名稱":
                            query = query.OrderBy(x => x.客戶資料.客戶名稱);
                            break;
                    }
                }
                else
                {
                    switch (column)
                    {
                        case "職稱":
                            query = query.OrderByDescending(x => x.職稱);
                            break;
                        case "姓名":
                            query = query.OrderByDescending(x => x.姓名);
                            break;
                        case "Email":
                            query = query.OrderByDescending(x => x.Email);
                            break;
                        case "手機":
                            query = query.OrderByDescending(x => x.手機);
                            break;
                        case "電話":
                            query = query.OrderByDescending(x => x.電話);
                            break;
                        case "客戶名稱":
                            query = query.OrderByDescending(x => x.客戶資料.客戶名稱);
                            break;
                    }
                }
            }
            return View(query.ToList());
        }


        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult BatchUpdate(IList<ContactBatchUpdateViewModel> data)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var contact = contactRepository.Find(item.Id);
                    contact.職稱 = item.職稱;
                    contact.手機 = item.手機;
                    contact.電話 = item.電話;
                }
                contactRepository.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.columnList = new SelectList(columnList, "Value", "Text");
            ViewBag.orderList = new SelectList(orderList, "Value", "Text");
            ViewData.Model = contactRepository.All();
            return View("Index");
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            bool hasSameEmail = db.客戶聯絡人.Where(x => x.客戶Id == 客戶聯絡人.客戶Id && x.Email == 客戶聯絡人.Email).Count() > 0;
            if (hasSameEmail)
            {
                ModelState.AddModelError("Email", "Has same Email");
            }
            if (ModelState.IsValid)
            {
                db.客戶聯絡人.Add(客戶聯絡人);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            bool hasSameEmail = db.客戶聯絡人.Where(x => x.客戶Id == 客戶聯絡人.客戶Id && x.Email == 客戶聯絡人.Email).Count() > 0;
            if (hasSameEmail)
            {
                ModelState.AddModelError("Email", "Has same Email");
            }
            if (ModelState.IsValid)
            {
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人.是否已刪除 = true;
            db.Entry(客戶聯絡人).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
