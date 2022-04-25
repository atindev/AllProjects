using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    public interface IEmailFormatService
    {
        Task<string> FormatEmail(Object mailObject, string blobName);
    }
}
