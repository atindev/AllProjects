using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Web.Models
{
    public class VerifyOtp
    {
        public string Email { get; set; }

        public string OtpFirstDigit { get; set; }

        public string OtpSecondDigit { get; set; }

        public string OtpThirdDigit { get; set; }

        public string OtpFourthDigit { get; set; }

        public string OtpFifthDigit { get; set; }

        public string OtpSixthDigit { get; set; }

        public string RemovePersonalEmail { get; set; }

    }
}
