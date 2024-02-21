using ATS.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static ATS.Models.RQRS;

namespace ATS.Controllers
{
    public class APIController : ApiController
    {
        // GET: API
        public RQRS.ResponseData Cand_details_srvc(RQRS.cand_details get_cand_details)
        {
            DataSet dsOutput = new DataSet();
            RQRS.ResponseData ResponseData = new RQRS.ResponseData();
            string strErrorMsg = string.Empty;
            try
            {
                Hashtable hs_Param = new Hashtable();
                hs_Param.Add("CANDIDATE_NAME", get_cand_details.CANDIDATE_NAME);
                hs_Param.Add("MOBILE", get_cand_details.MOBILE);
                hs_Param.Add("EMAIL", get_cand_details.EMAIL);
                hs_Param.Add("STATUS", get_cand_details.STATUS);
                hs_Param.Add("REMARKS", get_cand_details.REMARKS);
                hs_Param.Add("APP_FROM", get_cand_details.APP_FROM);
                hs_Param.Add("HR_NAME", get_cand_details.HR_NAME);
                hs_Param.Add("CV_BYTES", get_cand_details.CV_BYTES);
                hs_Param.Add("CV_NAME", get_cand_details.CV_NAME);

                int ResultCount = get_cand_details.CV_BYTES.Length;

                bool result = DBHandler.InsertUpdateDelete(DBHandler.StoreProcedure.CANDIDATES_DETAILS_SERVICE, hs_Param, ref strErrorMsg, ref ResultCount);
                if (ResultCount > 0)
                {
                    ResponseData.strStatus = "01";
                    ResponseData.strResponse = "Candidate details successfully inserted!";
                }
                else
                {
                    if (strErrorMsg != "")
                    {
                        ResponseData.strStatus = "00";
                        ResponseData.strResponse = strErrorMsg;
                        ResponseData.strErrorMessage = strErrorMsg;
                    }
                    else
                    {
                        ResponseData.strStatus = "00";
                        ResponseData.strResponse = "Unable to Insert now, Please Try again!";
                        ResponseData.strErrorMessage = "Problem occured while Insert!";
                    }
                }
            }
            catch (Exception ex)
            {
                ResponseData.strStatus = "00";
                ResponseData.strResponse = "Unable to connect database(#05)";
                ResponseData.strErrorMessage = Convert.ToString(ex);
            }
            return ResponseData;
        }
        public RQRS.ResponseData Upadate_Status(RQRS.Update_status update_Status)
        {
            DataSet dsOutput = new DataSet();
            RQRS.ResponseData ResponseData = new RQRS.ResponseData();
            string strErrorMsg = string.Empty;
            try
            {
                Hashtable hs_Param = new Hashtable();
                hs_Param.Add("CANDIDATE_ID", update_Status.CANDIDATE_ID);
                hs_Param.Add("STATUS", update_Status.STATUS);
                hs_Param.Add("REMARKS", update_Status.REMARKS);

                int ResultCount = 0;

                bool result = DBHandler.InsertUpdateDelete(DBHandler.StoreProcedure.UPDATE_STATUS, hs_Param, ref strErrorMsg, ref ResultCount);
                if (ResultCount > 0)
                {
                    ResponseData.strStatus = "01";
                    ResponseData.strResponse = "Candidate details Updated successfully!";
                }
                else
                {
                    if (strErrorMsg != "")
                    {
                        ResponseData.strStatus = "00";
                        ResponseData.strResponse = strErrorMsg;
                        ResponseData.strErrorMessage = strErrorMsg;
                    }
                    else
                    {
                        ResponseData.strStatus = "00";
                        ResponseData.strResponse = "Unable to Update now, Please Try again!";
                        ResponseData.strErrorMessage = "Problem occured while Insert!";
                    }
                }
            }
            catch (Exception ex)
            {
                ResponseData.strStatus = "00";
                ResponseData.strResponse = "Unable to connect database(#05)";
                ResponseData.strErrorMessage = Convert.ToString(ex);
            }
            return ResponseData;
        }

        public RQRS.ResponseData Check_Duplicate_srvc(RQRS.check_duplicate check_Duplicate)
        {
            DataSet dsOutput = new DataSet();
            RQRS.ResponseData ResponseData = new RQRS.ResponseData();
            string strErrorMsg = string.Empty;
            try
            {
                Hashtable hs_Param = new Hashtable();
                hs_Param.Add("MOBILE", check_Duplicate.MOBILE);
                hs_Param.Add("EMAIL", check_Duplicate.EMAIL);
                dsOutput = DBHandler.ExecProcedureReturnsDataset(DBHandler.StoreProcedure.CHECK_DUPLICATE_SERVICE, hs_Param, ref strErrorMsg);
                if (dsOutput != null && dsOutput.Tables.Count > 0 && dsOutput.Tables[0].Rows.Count > 0)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dsOutput.Tables[0]);
                    ResponseData.strStatus = "01";
                    ResponseData.strResponse = JSONString;
                }
                else
                {
                    ResponseData.strStatus = "00";
                    ResponseData.strResponse = "Candidate profile not found";
                    ResponseData.strErrorMessage = strErrorMsg != "" ? strErrorMsg : "No Records found";
                }
            }
            catch (Exception ex)
            {
                ResponseData.strStatus = "00";
                ResponseData.strResponse = "Unable to connect database(#05)";
                ResponseData.strErrorMessage = Convert.ToString(ex);
            }

                return ResponseData;
        }
        public RQRS.ResponseData Candidates_Details()
        {
            DataSet dsOutput = new DataSet();
            RQRS.ResponseData ResponseData = new RQRS.ResponseData();
            string strErrorMsg = string.Empty;
            try
            {
                Hashtable hs_Param = new Hashtable();
                dsOutput = DBHandler.ExecProcedureReturnsDataset(DBHandler.StoreProcedure.CANDIDATES_LIST, hs_Param, ref strErrorMsg);
                if (dsOutput != null && dsOutput.Tables.Count > 0 && dsOutput.Tables[0].Rows.Count > 0)
                {
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(dsOutput.Tables[0]);
                    ResponseData.strStatus = "01";
                    ResponseData.strResponse = JSONString;
                }
                else
                {
                    ResponseData.strStatus = "00";
                    ResponseData.strResponse = "Candidate profile not found";
                    ResponseData.strErrorMessage = strErrorMsg != "" ? strErrorMsg : "No Records found";
                }
            }
            catch (Exception ex)
            {
                ResponseData.strStatus = "00";
                ResponseData.strResponse = "Unable to connect database(#05)";
                ResponseData.strErrorMessage = Convert.ToString(ex);
            }

            return ResponseData;
        }

    }
}