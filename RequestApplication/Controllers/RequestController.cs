using RequestApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequestApplication.Controllers
{
    public class RequestController : Controller
    {


        RequestsDBEntities db = new RequestsDBEntities();

        // GET: Request
        public ActionResult Index()
        {
            return View();
        }

        //passes the list of table elements to the view called ListRequests to be displayed...works
        public ActionResult ListRequests()
        {

            List<RequestTable> RequestList = db.RequestTables.ToList();

            List<RequestTable> ListReq = RequestList.Select(x => new RequestTable {

                Title = x.Title,
                Description = x.Description,
                RequestDateArrival = x.RequestDateArrival,
                RequestRegistered = x.RequestRegistered,
                RequestFinished = x.RequestFinished,
                DocumentName = x.DocumentName,
                DocumentContent = x.DocumentContent,
                ID_Request = x.ID_Request

            }).ToList();

            return View(ListReq);
        }

        //it is used to open the form when edit pressed
        public ActionResult EditRequest(int? id)
        {
            RequestTable EditRequest = db.RequestTables.Where(r => r.ID_Request == id).FirstOrDefault();

            return View(EditRequest);

        }


        [HttpPost]
        public ActionResult SaveRequest(RequestTable model) {

            using (RequestsDBEntities context = new RequestsDBEntities())
            {


                RequestTable EditRequest = new RequestTable();     
                try
                {
                    if (ModelState.IsValid || model != null)
                    {
                        EditRequest.Title = "Alba";
                        EditRequest.Description = "from contr";
                        EditRequest.RequestDateArrival = DateTime.Now.ToShortDateString();
                        //EditRequest.RequestFinished = model.RequestFinished;
                        //EditRequest.RequestRegistered = model.RequestRegistered;
                        EditRequest.DocumentName = "file";
                        EditRequest.DocumentContent = "file desc";

                        db.RequestTables.AddOrUpdate(EditRequest);
                        db.SaveChanges();

                    }

                }
                catch (Exception ex)
                {
                    return Content("" + ex);
                }
            }


            return Content("saving request");
        }

        public ActionResult testing()
        {

            using (RequestsDBEntities context = new RequestsDBEntities())
            {
               RequestTable EditRequest = new RequestTable
                {
                    Title = "title",
                    Description = "from contr",
                    RequestDateArrival =DateTime.Now.ToShortDateString(),
                    //EditRequest.RequestFinished = model.RequestFinished;
                    //EditRequest.RequestRegistered = model.RequestRegistered;
                    DocumentName = "file",
                    DocumentContent = "file desc"
                };
                db.RequestTables.Add(EditRequest);
                db.SaveChanges();
            }
            return Content("saving in db");
        }
    }

    
}