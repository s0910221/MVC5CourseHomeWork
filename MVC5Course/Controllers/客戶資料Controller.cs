using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ClosedXML.Excel;
using MVC5Course.Helper;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;

namespace MVC5Course.Controllers
{
    public class 客戶資料Controller : Controller
    {
        private CustomerEntities db = new CustomerEntities();
        private IEnumerable<SelectListItem> customerTypeList;
        private 客戶資料Repository customerRepository;

        public 客戶資料Controller()
        {
            customerRepository = RepositoryHelper.Get客戶資料Repository();
            customerTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Diamond", Text = "Diamond" },
                new SelectListItem { Value = "Platinum", Text = "Platinum" },
                new SelectListItem { Value = "Gold", Text = "Gold" },
                new SelectListItem { Value = "Silver", Text = "Silver" },
                new SelectListItem { Value = "General", Text = "General" }
            };
        }
        // GET: 客戶資料
        public ActionResult Index()
        {
            ViewBag.customerType = new SelectList(customerTypeList, "Value", "Text");
            return View(Mapper.Map<List<CustomerCountViewModel>>(customerRepository.All().Where(x => !x.是否已刪除).ToList()));
        }

        [HttpPost]
        public ActionResult Index(string searchText, string customerType)
        {
            ViewBag.customerType = new SelectList(customerTypeList, "Value", "Text");
            if (String.IsNullOrEmpty(customerType))
                return View(Mapper.Map<List<CustomerCountViewModel>>(customerRepository.All().Where(x => x.客戶名稱.Contains(searchText) && !x.是否已刪除).ToList()));
            else
                return View(Mapper.Map<List<CustomerCountViewModel>>(customerRepository.All().Where(x => x.客戶名稱.Contains(searchText) && x.客戶分類 == customerType && !x.是否已刪除).ToList()));
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepository.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        public ActionResult Details_ContactList(int id)
        {
            ViewData.Model = customerRepository.Find(id).客戶聯絡人.Where(c => c.是否已刪除 == false).ToList();
            return View("ContactList");
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類 = new SelectList(customerTypeList, "Value", "Text");
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                customerRepository.Add(客戶資料);
                customerRepository.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶分類 = new SelectList(customerTypeList, "Value", "Text");
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepository.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶分類 = new SelectList(customerTypeList, "Value", "Text", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.客戶分類 = new SelectList(customerTypeList, "Value", "Text", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 customer = customerRepository.Find(id);
            customerRepository.Delete(customer);
            customerRepository.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult DownloadXls()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var data = Mapper.Map<List<CustomerCountViewModel>>(db.客戶資料.Where(x => !x.是否已刪除).ToList());
                var ws = wb.Worksheets.Add("cusdata", 1);
                int colIdx = 1;
                foreach (var item in typeof(CustomerCountViewModel).GetProperties())
                {
                    ws.Cell(1, colIdx++).Value = item.Name;
                }

                ws.Cell(2, 1).InsertData(data);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return File(memoryStream.ToArray(), "application/vnd.ms-excel", "Download.xlsx");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerRepository.UnitOfWork.Context.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
