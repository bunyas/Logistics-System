using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mascis;

using Syncfusion.JavaScript.DataSources;
using Syncfusion.JavaScript.Shared.Serializer;
using System.Web.Script.Serialization;
using Syncfusion.JavaScript.Models;
using Syncfusion.JavaScript;
using System.Collections;
using System.Reflection;
using Syncfusion.EJ;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using mascis.Models;
using Newtonsoft.Json;

namespace mascis.Controllers
{
    public class A_ProductController : Controller
    {
        private mascisEntities db = new mascisEntities();
        public IEnumerable currentData;
        string Module;
        // GET: A_Product
        public ActionResult Index()
        {
            var a_Product = db.A_Product.Include(a => a.A_product_category);
            return View(a_Product.ToList());
        }

        public ActionResult QueryHSIP()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetOrderProducts(DataManager dm, string startdate, string enddate)
        {
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate))
            {
                return View();
            }
            else
            {
                DateTime SD = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime ED = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                ProductModel obj = new ProductModel();
                var data = obj.GetOrders(SD, ED, null).ToList();
                int count = obj.GetOrders(SD, ED, null).Count();
                //DataOperations operation = new DataOperations();
                ////Performing filtering operation
                //if (dm.Where != null)
                //{
                //    data = operation.PerformWhereFilter(data, dm.Where, "and");
                //    var filtered = (IEnumerable<object>)data;
                //    count = filtered.Count();
                //}
                ////Performing search operation
                //if (dm.Search != null)
                //{
                //    data = operation.PerformSearching(data, dm.Search);
                //    var searched = (IEnumerable<object>)data;
                //    count = searched.Count();
                //}
                ////Performing sorting operation
                //if (dm.Sorted != null)
                //    data = operation.PerformSorting(data, dm.Sorted);

                ////Performing paging operations
                //if (dm.Skip != 0)
                //    data = operation.PerformSkip(data, dm.Skip);
                //if (dm.Take != 0)
                //    data = operation.PerformTake(data, dm.Take);

                //return Json(data, JsonRequestBehavior.AllowGet);
                var serializer = new JavaScriptSerializer();

                // For simplicity just use Int32's max value.
                // You could always read the value from the config section mentioned above.
                serializer.MaxJsonLength = Int32.MaxValue;

                //var resultData = new { Value = "foo", Text = "var" };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(data),
                    ContentType = "application/json"
                };
                return result;
            }

        }
        public ActionResult Products()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var yesno = db.A_yes_no.Where(a => a.yes_no_id == 1 || a.yes_no_id == 2).AsNoTracking().ToArray();
            foreach (A_yes_no Ayx in yesno)
            {
                //Capitalise the first character
                Ayx.yes_no_desc = new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(Ayx.yes_no_desc.ToLower());
            }
            ViewBag.YesNo = yesno;

            var drugBasicUnits = db.A_DrugBasicUnit.AsNoTracking().ToArray();
            ViewBag.DrugBasicUnits = drugBasicUnits;

            var product_Category = db.A_product_category.AsNoTracking().ToArray();
            ViewBag.product_Category = product_Category;

            var productStatus = db.A_ProductStatus.AsNoTracking().ToArray();
            ViewBag.productStatus = productStatus;

            var productType = db.A_ProductType.AsNoTracking().ToArray();
            ViewBag.productType = productType;
            var productClass = db.A_Product_Classification.AsNoTracking().ToArray();
            ViewBag.productClass = productClass;

            // var a_Product = db.A_Product.Include(a => a.A_product_category);

            //var DataSource = db.A_Product.ToList();
            //ViewBag.datasource = DataSource;


            return View(/*a_Product.ToList()*/);
        }

        public ActionResult BatchData(DataManager dataManager)
        {
            db.Configuration.ProxyCreationEnabled = false;
            IEnumerable data = db.A_Product.ToList();
            int count = db.A_Product.ToList().Count();


            db.Configuration.ProxyCreationEnabled = false;
            DataOperations operation = new DataOperations();
            //Performing filtering operation
            if (dataManager.Where != null)
            {
                data = operation.PerformWhereFilter(data, dataManager.Where, "and");
                var filtered = (IEnumerable<object>)data;
                count = filtered.Count();
            }
            //Performing search operation
            if (dataManager.Search != null)
            {
                data = operation.PerformSearching(data, dataManager.Search);
                var searched = (IEnumerable<object>)data;
                count = searched.Count();
            }
            //Performing sorting operation
            if (dataManager.Sorted != null)
                data = operation.PerformSorting(data, dataManager.Sorted);

            //Performing paging operations
            if (dataManager.Skip > 0)
                data = operation.PerformSkip(data, dataManager.Skip);
            if (dataManager.Take > 0)
                data = operation.PerformTake(data, dataManager.Take);

            return Json(new { result = data, count = count }, JsonRequestBehavior.AllowGet);

        }

        public class DataResult
        {
            public IEnumerable result { get; set; }
            public int count { get; set; }
        }

        public ActionResult DialogInsert(A_Product value/*, string testdata, string testdata2, string testdata3*/)
        {
            try
            {
                var _feedback = db.A_Product.FirstOrDefault(f => f.product_code == value.product_code);
                db.Configuration.ProxyCreationEnabled = false;

                if (_feedback == null)
                {

                    db.A_Product.Add(value);
                }
                else
                {
                    db.Entry(_feedback).CurrentValues.SetValues(value);
                    db.Entry(_feedback).State = EntityState.Modified;
                }
                if (value.Basic_Unit == 0)
                {
                    value.Basic_Unit = null;
                }
                var recsaved = db.SaveChanges();
                // msg = value.product_description + " Saved Successfully";
                return Json(new
                {
                    msg = "Successfully added " + value.product_description,
                    JsonRequestBehavior.AllowGet
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }


            // return Json(msg, JsonRequestBehavior.AllowGet);
        }

        // GET: A_Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Product a_Product = db.A_Product.Find(id);
            if (a_Product == null)
            {
                return HttpNotFound();
            }
            return View(a_Product);
        }

        // GET: A_Product/Create
        public ActionResult Create()
        {
            ViewBag.product_category = new SelectList(db.A_product_category, "category_code", "category_desc");
            return View();
        }

        // POST: A_Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_code,product_description,strength,packsize,carton_size_pkt,carton_weight_kg,product_category,Basic_Unit,approved_MOH_NTG,approved_WHO,approved_FDA,nda_registration,SAP_code,ConsummableProduct,ART_product_classification,NMS_Codes,ProductStatusId,ProductExpires,TracerCommodity,ProductTypeId")] A_Product a_Product)
        {
            if (ModelState.IsValid)
            {
                db.A_Product.Add(a_Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_category = new SelectList(db.A_product_category, "category_code", "category_desc", a_Product.product_category);
            return View(a_Product);
        }

        // GET: A_Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Product a_Product = db.A_Product.Find(id);
            if (a_Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_category = new SelectList(db.A_product_category, "category_code", "category_desc", a_Product.product_category);
            return View(a_Product);
        }

        // POST: A_Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_code,product_description,strength,packsize,carton_size_pkt,carton_weight_kg,product_category,Basic_Unit,approved_MOH_NTG,approved_WHO,approved_FDA,nda_registration,SAP_code,ConsummableProduct,ART_product_classification,NMS_Codes,ProductStatusId,ProductExpires,TracerCommodity,ProductTypeId")] A_Product a_Product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_category = new SelectList(db.A_product_category, "category_code", "category_desc", a_Product.product_category);
            return View(a_Product);
        }

        // GET: A_Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_Product a_Product = db.A_Product.Find(id);
            if (a_Product == null)
            {
                return HttpNotFound();
            }
            return View(a_Product);
        }

        // POST: A_Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            A_Product a_Product = db.A_Product.Find(id);
            db.A_Product.Remove(a_Product);
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
        public void ExportToExcel(string GridModel)
        {
            currentData = db.A_Product.ToList();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            obj.Columns[5].DataSource = db.A_product_category.ToList();
            obj.Columns[15].DataSource = db.A_ProductStatus.ToList();
            obj.Columns[6].DataSource = db.A_DrugBasicUnit.ToList();
            obj.Columns[18].DataSource = db.A_ProductType.ToList();
            obj.Columns[19].DataSource = db.A_Product_Classification.ToList();
            //GridProperties obj = (GridProperties)Syncfusion.JavaScript.Utils.DeserializeToModel(typeof(GridProperties), GridModel);
            exp.Export(obj, currentData, Module + ".xlsx", ExcelVersion.Excel2013, false, false, "flat-saffron");
        }
        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (ds.Key == "ProductsData")
                {
                    string str = Convert.ToString(ds.Value);
                   // currentData = JsonConvert.DeserializeObject<IEnumerable<A_Product>>(str);
                    Module = "Medical Access Order Query All Products" + DateTime.Now.ToLongDateString();
                    continue;
                }

                if (property != null)
                {
                    Type type = property.PropertyType;
                    string serialize = serializer.Serialize(ds.Value);
                    object value = serializer.Deserialize(serialize, type);
                    property.SetValue(gridProp, value, null);
                }
            }
            return gridProp;
        }
    }
}
