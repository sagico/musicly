using System;

namespace musicly.Models
{
    public class ErrorViewModel
    {
        //public string RequestId { get; set; }

        //public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorCode { get; set; }

        public string Description { get; set; }
    }
}