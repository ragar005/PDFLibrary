﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private PDFLibraryCaller _pdfCaller;


        public HomeController(PDFLibraryCaller pdfCaller)
        {
            _pdfCaller = pdfCaller;
        }


        [HttpPost]
        public ActionResult UploadPDF(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
            {
                int fileSizeInBytes = file.ContentLength;
                byte[] pdf = null;
                using (var br = new BinaryReader(file.InputStream))
                {
                    pdf = br.ReadBytes(fileSizeInBytes);
                }

                var data = PDFLibrary.Main.GetData(pdf);

                var pdfData = new PDFData();

                pdfData.FirstName = data["FirstName"];
                pdfData.MiddleInitial = data["MiddleInitial"];
                pdfData.LastName = data["LastName"];
                pdfData.Street = data["Street"];
                pdfData.City = data["City"];
                pdfData.State = data["State"];
                pdfData.Zip = data["Zip"];
                pdfData.CustomerSince = data["CustomerSince"];
                pdfData.PointBalance = data["PointBalance"];
                pdfData.TIN = data["TIN"];
                pdfData.Active = data["Active"] == "Yes" ? true : false;

                TempData["Model"] = pdfData;


                return RedirectToAction("CreatePDF");

            }



            return View();
        }


        public ActionResult UploadPDF()
        {
            return View();
        }




        public ActionResult CreatePDF()
        {



            string state = null;
            if (TempData["States"] != null)
                state = ((PDFData) TempData["States"]).State;

            TempData["States"] = getStates(state);

            return TempData["Model"] != null ? View(TempData["Model"]) : View();


        }

        [HttpPost]
        public ActionResult CreatePDF(PDFData pdfData)
        {
          return new FileContentResult(_pdfCaller.GetPDF(pdfData, Server.MapPath("~/PDFs/Target.pdf")), "application/pdf");
        }


        List<SelectListItem> getStates(string state = null)
        {
            var states =   new List<SelectListItem>()
    {
        new SelectListItem() {Text="Alabama", Value="AL"},
        new SelectListItem() { Text="Alaska", Value="AK"},
        new SelectListItem() { Text="Arizona", Value="AZ"},
        new SelectListItem() { Text="Arkansas", Value="AR"},
        new SelectListItem() { Text="California", Value="CA"},
        new SelectListItem() { Text="Colorado", Value="CO"},
        new SelectListItem() { Text="Connecticut", Value="CT"},
        new SelectListItem() { Text="District of Columbia", Value="DC"},
        new SelectListItem() { Text="Delaware", Value="DE"},
        new SelectListItem() { Text="Florida", Value="FL"},
        new SelectListItem() { Text="Georgia", Value="GA"},
        new SelectListItem() { Text="Hawaii", Value="HI"},
        new SelectListItem() { Text="Idaho", Value="ID"},
        new SelectListItem() { Text="Illinois", Value="IL"},
        new SelectListItem() { Text="Indiana", Value="IN"},
        new SelectListItem() { Text="Iowa", Value="IA"},
        new SelectListItem() { Text="Kansas", Value="KS"},
        new SelectListItem() { Text="Kentucky", Value="KY"},
        new SelectListItem() { Text="Louisiana", Value="LA"},
        new SelectListItem() { Text="Maine", Value="ME"},
        new SelectListItem() { Text="Maryland", Value="MD"},
        new SelectListItem() { Text="Massachusetts", Value="MA"},
        new SelectListItem() { Text="Michigan", Value="MI"},
        new SelectListItem() { Text="Minnesota", Value="MN"},
        new SelectListItem() { Text="Mississippi", Value="MS"},
        new SelectListItem() { Text="Missouri", Value="MO"},
        new SelectListItem() { Text="Montana", Value="MT"},
        new SelectListItem() { Text="Nebraska", Value="NE"},
        new SelectListItem() { Text="Nevada", Value="NV"},
        new SelectListItem() { Text="New Hampshire", Value="NH"},
        new SelectListItem() { Text="New Jersey", Value="NJ"},
        new SelectListItem() { Text="New Mexico", Value="NM"},
        new SelectListItem() { Text="New York", Value="NY"},
        new SelectListItem() { Text="North Carolina", Value="NC"},
        new SelectListItem() { Text="North Dakota", Value="ND"},
        new SelectListItem() { Text="Ohio", Value="OH"},
        new SelectListItem() { Text="Oklahoma", Value="OK"},
        new SelectListItem() { Text="Oregon", Value="OR"},
        new SelectListItem() { Text="Pennsylvania", Value="PA"},
        new SelectListItem() { Text="Rhode Island", Value="RI"},
        new SelectListItem() { Text="South Carolina", Value="SC"},
        new SelectListItem() { Text="South Dakota", Value="SD"},
        new SelectListItem() { Text="Tennessee", Value="TN"},
        new SelectListItem() { Text="Texas", Value="TX"},
        new SelectListItem() { Text="Utah", Value="UT"},
        new SelectListItem() { Text="Vermont", Value="VT"},
        new SelectListItem() { Text="Virginia", Value="VA"},
        new SelectListItem() { Text="Washington", Value="WA"},
        new SelectListItem() { Text="West Virginia", Value="WV"},
        new SelectListItem() { Text="Wisconsin", Value="WI"},
        new SelectListItem() { Text="Wyoming", Value="WY"}
    };
            if (state != null)
               states.First(x => x.Value == state).Selected = true;

            return states;

        }


    }
}