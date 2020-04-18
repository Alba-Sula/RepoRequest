using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RequestApplication.Models
{
    public class RequestViewModel
    {
        public int ID_Request { get; set; }
        [Display(Name = "Request Title")]
        public string Title { get; set; }
        [Display(Name = "Request Description")]
        public string Description { get; set; }
        [Display(Name = "Request Date Arrival")]
        public DateTime? RequestDateArrival { get; set; }
        [Display(Name = "Request Registered Date")]
        public string RequestRegistered { get; set; }
        [Display(Name = "Request Finished Date")]
        public DateTime? RequestFinished { get; set; }
        [Display(Name = "Request File Name")]
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public Nullable<int> ID_Status { get; set; }
        [Display(Name = "Request Status")]
        public string TitleStatus { get; set; }
        [Display(Name = "Request Status Description")]
        public string DescriptionStatus { get; set; }
    }
}