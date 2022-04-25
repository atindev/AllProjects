using Microsoft.Extensions.Options;
using Twilio;
using Twilio.TwiML;
using Twilio.Rest.Api.V2010.Account;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;
using Twilio.Rest.Verify.V2.Service;
using System.Threading.Tasks;
using Twilio.Exceptions;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;


namespace WAS.Infrastructure.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly TwilioSettings _twilioSettings;
         
        public TwilioService(IOptions<TwilioSettings> twilioSettings)
        {
            _twilioSettings = twilioSettings.Value;
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
        }

        /// <summary>
        /// Send SMS using twilio account
        /// </summary>
        /// <param name="notification">SMS body and to phone number</param>
        /// <returns>Message resource object</returns>
        public async Task<Twilio.Rest.Api.V2010.Account.MessageResource> SendSMS(TwilioSms notification)
        {
                var message = await Twilio.Rest.Api.V2010.Account.MessageResource.CreateAsync(
                    body: notification.Body.Trim(),
                    from: new Twilio.Types.PhoneNumber(notification.FromNumber),
                    to: new Twilio.Types.PhoneNumber(notification.ToNumber)
                );
                return message;
        }

        /// <summary>
        /// Send whatsapp message using twilio account
        /// </summary>
        /// <param name="notification">whatsapp message body and to phone number</param>
        /// <returns>Message resource object</returns>
        public async Task<Twilio.Rest.Api.V2010.Account.MessageResource> SendWhatsAppMessage(TwilioSms notification)
        {
                var message = await Twilio.Rest.Api.V2010.Account.MessageResource.CreateAsync(
                 body: notification.Body.Trim(),
                 from: new Twilio.Types.PhoneNumber($"whatsapp:{notification.FromNumber}"),
                 to: new Twilio.Types.PhoneNumber($"whatsapp:{notification.ToNumber}")
             );
                return message;
        }

        /// <summary>
        /// Fetch Delivery Report of a particular SMS
        /// </summary>
        /// <param name="sid">SMS Id</param>
        /// <returns>Delivery Status</returns>
        public async Task<DeliveryReportStatus> FetchMessageDeliveryReport(string sid)
        {
            try
            {
                var message = await Twilio.Rest.Api.V2010.Account.MessageResource.FetchAsync(sid);
                if (message != null)
                {
                    var ErrorCode = (message.ErrorCode != null && message.ErrorCode != 0) ? message.ErrorCode : null;
                    return new DeliveryReportStatus() { Status = message.Status.ToString(), ErrorMessage = message.ErrorMessage, ToNumber = message.To, ErrorCode = ErrorCode };
                }
                else
                    return new DeliveryReportStatus() { Status = "404", ErrorMessage = "", ToNumber = "", ErrorCode = null };
            }
            catch (TwilioException ex)
            {
                return new DeliveryReportStatus() { Status = "404", ErrorMessage = ex.Message, ToNumber = "", ErrorCode = null };
            }
        }

        /// <summary>
        /// Send Voice using twilio account
        /// </summary>
        /// <param name="notification">Voice body and to phone number</param>
        /// <returns>Call resource object</returns>
        public async Task<CallResource> SendVoice(TwilioVoice notification)
        {
                var call = await CallResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber(notification.FromNumber),
                    to: new Twilio.Types.PhoneNumber(notification.ToNumber),
                    twiml: new Twilio.Types.Twiml(new VoiceResponse().Say(notification.Body, voice: "alice", loop: notification.RepeatCount, language: notification.Language).ToString())
                );
                return call;
        }

        /// <summary>
        /// Fetch Delivery Report of a particular Call
        /// </summary>
        /// <param name="callId">SMS Id</param>
        /// <returns>Delivery Status</returns>
        public async Task<DeliveryReportStatus> FetchCallDeliveryReport(string callId)
        {
            var call = await CallResource.FetchAsync(callId);
            return new DeliveryReportStatus() { Status = call.Status.ToString(), ErrorMessage = null, ToNumber = call.To , ErrorCode=null};
        }

        /// <summary>
        /// Send OTP to the resource
        /// </summary>
        /// <param name="EmailVerification">To Email</param>
        /// <returns>Verification Result</returns>
        public async Task<TwilioVerificationResult> SendOtpAsync(TwilioVerificationResource twilioVerificationResource)
        {
            try
            {
                var verificationResource = await VerificationResource.CreateAsync(
                    to: twilioVerificationResource.To,
                    channel: twilioVerificationResource.Channel,
                    pathServiceSid: _twilioSettings.ServiceSid
                );

                return new TwilioVerificationResult(verificationResource.Sid);
            }
            catch (TwilioException e)
            {
                return new TwilioVerificationResult(new List<string> { e.Message });
            }
        }

        /// <summary>
        /// Verfiy OTP
        /// </summary>
        /// <param name="EmailVerification">To Email and OTP</param>
        /// <returns>Verification Result</returns>
        public async Task<TwilioVerificationResult> VerifyOtpAsync(TwilioVerificationResource twilioVerificationResource)
        {
            try
            {
                var verificationCheckResource = await VerificationCheckResource.CreateAsync(
                    to: twilioVerificationResource.To,
                    code: twilioVerificationResource.Otp,
                    pathServiceSid: _twilioSettings.ServiceSid
                );

                return verificationCheckResource.Status.Equals("approved") ?
                    new TwilioVerificationResult(verificationCheckResource.Sid) :
                    new TwilioVerificationResult(new List<string> { "Wrong code. Try again." });
            }
            catch (TwilioException e)
            {
                return new TwilioVerificationResult(new List<string> { e.Message });
            }
        }
    }
}
