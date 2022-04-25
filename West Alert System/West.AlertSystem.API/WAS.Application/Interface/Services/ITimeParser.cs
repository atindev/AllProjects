using System;
using System.Collections.Generic;
using System.Text;
using Twilio.Rest.Api.V2010.Account;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface ITimeParser
    {
        string RelativeTime(DateTime dateTime);

        string GetTime(int totalseconds);
    }
}
