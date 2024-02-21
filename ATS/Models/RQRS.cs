using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATS.Models
{
    public class RQRS
    {
        #region Resopnse
        public class ResponseData
        {
            public string strStatus { get; set; }
            public string strResponse { get; set; }
            public string strErrorMessage { get; set; }

        };
        #endregion

        #region Request
        public class cand_details
        {
            public string CANDIDATE_NAME { get; set; }
            public string MOBILE { get; set; }
            public string EMAIL { get; set; }
            public string STATUS { get; set; }
            public string REMARKS { get; set; }
            public string APP_FROM { get; set; }
            public string HR_NAME { get; set; }
            public byte[] CV_BYTES { get; set; }
            public string CV_NAME { get; set; }
        };

        public class check_duplicate
        {
            public string MOBILE { get; set; }
            public string EMAIL { get; set; }
        }
        public class Update_status
        {
            public string CANDIDATE_ID { get; set; }
            public string STATUS { get; set; }
            public string REMARKS { get; set; }
        }

        #endregion
    }
}