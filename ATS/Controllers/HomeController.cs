using ATS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml.Linq;
using static ATS.Models.RQRS;
using static System.Net.WebRequestMethods;
using System.Net;

namespace ATS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CandidatesList()
        {
            return View();
        }

        public ActionResult test()
        {
            //Convert file into bytes
            string fname = "C:\\Users\\Prakash.K.CORPBRISTLECONE\\source\\repos\\ATS\\ATS\\TempFiles\\Problem Statements_Hack O' Holics.pdf";
            byte[] file = null;
            string varFilePath = fname;
            string extn = new FileInfo(varFilePath).Extension;
            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            //
            System.IO.File.WriteAllBytes("C:\\Users\\Prakash.K.CORPBRISTLECONE\\OneDrive - Mahindra & Mahindra Ltd.-55241918-Bristlecone India Pvt Ltd\\Desktop\\CheckTest.pdf", file);
            System.Diagnostics.Process.Start("C:\\Users\\Prakash.K.CORPBRISTLECONE\\OneDrive - Mahindra & Mahindra Ltd.-55241918-Bristlecone India Pvt Ltd\\Desktop\\CheckTest.pdf");
            //Convert bytes into file and save
            using (Stream file1 = System.IO.File.Create(@"C:\Users\Prakash.K.CORPBRISTLECONE\OneDrive - Mahindra & Mahindra Ltd.-55241918-Bristlecone India Pvt Ltd\Desktop\test12345"))
            {
                file1.Write(file, 0, file.Length);
            }
            //Save file
            string fileName = "Problem Statements_Hack O' Holics.pdf";
            string filePath = Server.MapPath(string.Format("~/TempFiles/{0}", fileName));

            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(filePath, userRoot + "\\Downloads\\test1232.pdf");
            }

            return Json(new { Status = "01", Message = "00" });
        }

        public ActionResult InsertCandidate(string strName, string strEmail, string strMobile, string strHRName, string strAppFrom)
        {
            string fname = string.Empty;
            byte[] file = null;
            string extn = string.Empty;
            string filename = string.Empty;
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResponse = string.Empty;
            try
            {                
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file1 = files[0];
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file1.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file1.FileName;
                }
                filename = fname;
                fname = Path.Combine(Server.MapPath("~/TempFiles/"), fname);
                file1.SaveAs(fname);
                string varFilePath = fname;
                extn = new FileInfo(varFilePath).Extension;
                using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }
                System.IO.File.Delete(fname);
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMessage = ex.ToString();
                return Json(new { Status = strStatus, Message = strMessage });
            }

            try
            {
                RQRS.cand_details cand_details = new RQRS.cand_details();
                cand_details.CANDIDATE_NAME = strName;
                cand_details.MOBILE = strMobile;
                cand_details.EMAIL = strEmail;
                cand_details.STATUS = "In process";
                cand_details.REMARKS = "Initiated";
                cand_details.HR_NAME = strHRName;
                cand_details.CV_BYTES = file;
                cand_details.CV_NAME = filename;
                cand_details.APP_FROM = strAppFrom;
                string strRequest = JsonConvert.SerializeObject(cand_details);
                strResponse = Baseclass.InvokeServiceReq("API/Cand_details_srvc", strRequest, "POST");
                RQRS.ResponseData Response = JsonConvert.DeserializeObject<RQRS.ResponseData>(strResponse);
                strStatus = Response.strStatus;
                strMessage = Response.strResponse;
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMessage = "Unable to connect Service(#05)";
            }
            return Json(new { Status = strStatus, Message = strMessage });
        }

        public ActionResult checkDuplicate(string strMobile, string strEmail)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResponse = string.Empty;

            try
            {
                RQRS.check_duplicate check_Duplicate = new RQRS.check_duplicate();
                check_Duplicate.MOBILE = strMobile;
                check_Duplicate.EMAIL = strEmail;
                string strRequest = JsonConvert.SerializeObject(check_Duplicate);
                strResponse = Baseclass.InvokeServiceReq("API/Check_Duplicate_srvc", strRequest, "POST");
                RQRS.ResponseData Response = JsonConvert.DeserializeObject<RQRS.ResponseData>(strResponse);
                strStatus = Response.strStatus;
                strMessage = Response.strResponse;
            }
            catch(Exception ex)
            {
                strStatus = "00";
                strMessage = "Unable to connect Service(#05)";
            }

            return Json(new { Status = strStatus, Message = strMessage });
        }
        public ActionResult UpdateStaus(string txtcandId, string txtStatus, string txtRemark)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResponse = string.Empty;

            try
            {
                RQRS.Update_status update_status = new RQRS.Update_status();
                update_status.CANDIDATE_ID = txtcandId;
                update_status.STATUS = txtStatus;
                update_status.REMARKS = txtRemark;
                string strRequest = JsonConvert.SerializeObject(update_status);
                strResponse = Baseclass.InvokeServiceReq("API/Upadate_Status", strRequest, "POST");
                RQRS.ResponseData Response = JsonConvert.DeserializeObject<RQRS.ResponseData>(strResponse);
                strStatus = Response.strStatus;
                strMessage = Response.strResponse;
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMessage = "Unable to connect Service(#05)";
            }

            return Json(new { Status = strStatus, Message = strMessage });
        }
        public ActionResult CandidateDetails()
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResponse = string.Empty;

            try
            {
                RQRS.check_duplicate check_Duplicate = new RQRS.check_duplicate();
                string strRequest = JsonConvert.SerializeObject(check_Duplicate);
                strResponse = Baseclass.InvokeServiceReq("API/Candidates_Details", strRequest, "POST");
                RQRS.ResponseData Response = JsonConvert.DeserializeObject<RQRS.ResponseData>(strResponse);
                strStatus = Response.strStatus;
                strMessage = Response.strResponse;
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMessage = "Unable to connect Service(#05)";
            }

            return Json(new { Status = strStatus, Message = strMessage });
        }
    }
}