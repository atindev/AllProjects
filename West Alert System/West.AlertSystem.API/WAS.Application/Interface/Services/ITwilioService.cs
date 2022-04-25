using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface ITwilioService
    {
        Task<MessageResource> SendSMS(TwilioSms notification);

        Task<MessageResource> SendWhatsAppMessage(TwilioSms notification);

        Task<DeliveryReportStatus> FetchMessageDeliveryReport(string sid);

        Task<CallResource> SendVoice(TwilioVoice notification);

        Task<DeliveryReportStatus> FetchCallDeliveryReport(string callId);

        Task<TwilioVerificationResult> SendOtpAsync(TwilioVerificationResource twilioVerificationResource);

        Task<TwilioVerificationResult> VerifyOtpAsync(TwilioVerificationResource twilioVerificationResource);
    }
}
