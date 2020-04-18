using Microsoft.Ajax.Utilities;
using RequestApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Validation;

namespace RequestApplication.Controllers
{
    public class RequestController : Controller
    {
        RequestsDBEntities db = new RequestsDBEntities();
        //List of requests
        public ActionResult ListRequests(int? page, string search)
        {
            List<RequestTable> RequestList = db.RequestTables.Include("Status").ToList();
            var ListReq = RequestList.Select(xb => new RequestViewModel
            {
                Title = xb.Title,
                Description = xb.Description,
                RequestDateArrival = xb.RequestDateArrival,
                RequestRegistered = xb.RequestRegistered,
                RequestFinished = xb.RequestFinished,
                ID_Request = xb.ID_Request,
                FileData = xb.FileData,
                FileName = xb.FileName,
                ID_Status = xb.ID_Status,
                TitleStatus = xb.Status.Title,

            }).ToList();

            List<RequestViewModel> requestVMList;

            if (search != null)
            {
                requestVMList = ListReq.Where(y => y.Title.Contains(search) || search == null).ToList();
            }

            else
            {
                requestVMList = ListReq;
            }
            return View(requestVMList.ToPagedList(page ?? 1, 10));
        }
        //Edit View
        public ActionResult EditRequest(int? id)
        {
            RequestTable EditRequest = db.RequestTables.Where(r => r.ID_Request == id).FirstOrDefault();

            return View(EditRequest);
        }

        //Saving the edited request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRequest(RequestTable model)
        {
            try
            {
                if (ModelState.IsValid || model != null)
                {
                    if (model.ID_Request > 0)
                    {
                        var EditRequest = db.RequestTables.Where(a => a.ID_Request == model.ID_Request).FirstOrDefault();
                        if (EditRequest != null)
                        {
                            EditRequest.Title = model.Title;
                            EditRequest.Description = model.Description;
                            EditRequest.RequestDateArrival = model.RequestDateArrival;
                            EditRequest.RequestFinished = model.RequestFinished;
                            EditRequest.Status.Title = model.Status.Title;
                            EditRequest.Status.Description = model.Status.Description;


                        }
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            return RedirectToAction("ListRequests");
        }

        public ActionResult NewRequest()
        {
            return View();
        }


        //Saving the request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavingNewRequest(RequestTable modelReq, HttpPostedFileBase postedFile)
        {
            if (!ModelState.IsValid)
            {
                return View("NewRequest");
            }
            else if (postedFile == null || postedFile.ContentLength < 0)
            {
                try
                {

                    var StatusRequest = new Status();

                    StatusRequest.Title = modelReq.Status.Title;
                    StatusRequest.Description = modelReq.Status.Description;

                    db.Status.Add(StatusRequest);
                    db.SaveChanges();

                    var idStatus = StatusRequest.ID_Status;
                    var EditRequest = new RequestTable();

                    EditRequest.Title = modelReq.Title;
                    EditRequest.Description = modelReq.Description;
                    EditRequest.RequestDateArrival = modelReq.RequestDateArrival;
                    EditRequest.RequestRegistered = DateTime.Now.ToString();
                    EditRequest.RequestFinished = modelReq.RequestFinished;
                    EditRequest.ID_Status = idStatus;

                    db.RequestTables.Add(EditRequest);
                    int row = db.SaveChanges();

                    return RedirectToAction("ListRequests");

                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            else
            {
                try
                {
                    int fileSize = postedFile.ContentLength;
                    string filetype = postedFile.ContentType;
                    var fileName = Path.GetFileName(postedFile.FileName);
                    byte[] bytes = new byte[fileSize];
                    postedFile.InputStream.Read(bytes, 0, fileSize);


                    var StatusRequest = new Status();

                    StatusRequest.Title = modelReq.Status.Title;
                    StatusRequest.Description = modelReq.Status.Description;

                    db.Status.Add(StatusRequest);
                    db.SaveChanges();

                    var idStatus = StatusRequest.ID_Status;
                    var EditRequest = new RequestTable();

                    EditRequest.Title = modelReq.Title;
                    EditRequest.Description = modelReq.Description;
                    EditRequest.RequestDateArrival = modelReq.RequestDateArrival;
                    EditRequest.RequestRegistered = DateTime.Now.ToString();
                    EditRequest.RequestFinished = modelReq.RequestFinished;
                    EditRequest.FileName = fileName;
                    EditRequest.FileData = bytes;
                    EditRequest.FileType = filetype;
                    EditRequest.ID_Status = idStatus;

                    db.RequestTables.Add(EditRequest);
                    int row = db.SaveChanges();
                    return RedirectToAction("ListRequests");
                }
                catch (Exception e)
                {
                    return View("Error");
                }
            }
        }
        //Deletes the request
        public ActionResult DeleteReq(int id)
        {
            RequestTable request = db.RequestTables.Find(id);
            if (request == null)
            {
                return View("Error");
            }

            db.RequestTables.Remove(request);
            db.SaveChanges();

            return RedirectToAction("ListRequests");
        }
        //Check if on click to download button you have a file to download... 
        //if you do than redirects you to download actionresult...
        //if not than it redirects you to the same page
        public RedirectToRouteResult IfDownload(int id)
        {
            RequestTable request = db.RequestTables.Find(id);
            if (request.FileName != null)
            {
                return RedirectToAction("DownloadFile", new { idRequest = id });
            }

            return RedirectToAction("ListRequests");
        }
        //Downloads the file
        public FileResult DownloadFile(int idRequest)
        {

            RequestTable request = db.RequestTables.Find(idRequest);
            var fileName = request.FileName;
            byte[] bytes = request.FileData;
            var fileType = request.FileType;

            return File(bytes, fileType, fileName);
        }
    }
}