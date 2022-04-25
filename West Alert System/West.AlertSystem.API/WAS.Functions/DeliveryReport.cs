using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Functions
{
    public class DeliveryReport
    {
        private readonly IWasContextAdmin _context;
        private readonly ITwilioService _twilioService;

        public DeliveryReport(
            IWasContextAdmin context,
            ITwilioService twilioService)
        {
            _context = context;
            _twilioService = twilioService;
        }

        [FunctionName("DeliveryReport")]
        public async Task Run([TimerTrigger("*/20 * * * * *")] TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            try
            {
                await GetDeliveryReport(cancellationToken);

                await GetFailedReport(cancellationToken);
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private async Task GetDeliveryReport(CancellationToken cancellationToken)
        {
            var messages = await _context.DeliveryReportTexts
                   .Where(dr => dr.Status != "delivered" && dr.Status != "failed" && dr.Status != "undelivered")
                   .ToListAsync(cancellationToken);

            var calls = await _context.DeliveryReportVoices
                .Where(dr => dr.Status != "completed" && dr.Status != "busy" && dr.Status != "no-answer" && dr.Status != "canceled" && dr.Status != "failed")
                .ToListAsync(cancellationToken);

            var whatsApps = await _context.DeliveryReportWhatsApps
               .Where(dr => dr.Status != "failed" && dr.Status != "read" && dr.Status != "undelivered")
               .ToListAsync(cancellationToken);

            await UpdateDeliveryReport(messages,calls,whatsApps, cancellationToken);
        }

        private async Task UpdateDeliveryReport(List<Domain.Entities.DeliveryReportText> messages, List<Domain.Entities.DeliveryReportVoice> calls, List<Domain.Entities.DeliveryReportWhatsApp> whatsApps, CancellationToken cancellationToken)
        {
            Parallel.ForEach(messages, text =>
            {
                var latestMessageStatus = _twilioService.FetchMessageDeliveryReport(text.SmsId);
                if (latestMessageStatus != null && latestMessageStatus.Result != null && latestMessageStatus.Result.Status != "404")
                {
                    text.Status = latestMessageStatus.Result.Status;
                    text.ErrorMessage = latestMessageStatus.Result.ErrorMessage;
                    text.ToNumber = latestMessageStatus.Result.ToNumber;
                }
            });

            Parallel.ForEach(calls, call =>
            {
                var latestCallStatus = _twilioService.FetchCallDeliveryReport(call.CallId);
                call.Status = latestCallStatus.Result.Status;
                call.ErrorMessage = latestCallStatus.Result.ErrorMessage;
                call.ToNumber = latestCallStatus.Result.ToNumber;
            });

            Parallel.ForEach(whatsApps, text =>
            {
                var latestWhatsAppMessageStatus = _twilioService.FetchMessageDeliveryReport(text.WhatsAppId);
                if (latestWhatsAppMessageStatus != null && latestWhatsAppMessageStatus.Result != null && latestWhatsAppMessageStatus.Result.Status != "404")
                {
                    text.Status = latestWhatsAppMessageStatus.Result.Status;
                    text.ErrorMessage = latestWhatsAppMessageStatus.Result.ErrorMessage;
                    string WhatsAppToNumber = latestWhatsAppMessageStatus.Result.ToNumber;
                    if (WhatsAppToNumber != null && WhatsAppToNumber != "")
                        WhatsAppToNumber = WhatsAppToNumber.Replace("whatsapp:", "");
                    text.ToNumber = WhatsAppToNumber;
                }
            });

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task GetFailedReport(CancellationToken cancellationToken)
        {

            //fetching delivery report for failed and undelivered messages
            var failedwhatsAppMessage = await _context.DeliveryReportWhatsApps
               .Where(dr => (dr.Status == "failed" || dr.Status == "undelivered") &&
               (dr.ErrorCode == null || dr.ErrorMessage == null) &&
               (dr.CreatedDate.AddMinutes(30) > DateTime.UtcNow)
               )
               .ToListAsync(cancellationToken);

            var failedTextMessage = await _context.DeliveryReportTexts
              .Where(dr => (dr.Status == "failed" || dr.Status == "undelivered") &&
              (dr.ErrorCode == null || dr.ErrorMessage == null) &&
              (dr.CreatedDate.AddMinutes(30) > DateTime.UtcNow)
              )
              .ToListAsync(cancellationToken);

            await UpdateFailedDeliveryReportText(failedTextMessage,cancellationToken);

            await UpdateFailedDeliveryReportWhatsapp(failedwhatsAppMessage, cancellationToken);
        }

        private async Task UpdateFailedDeliveryReportText(List<Domain.Entities.DeliveryReportText> failedTextMessage, CancellationToken cancellationToken)
        {
            Parallel.ForEach(failedTextMessage, text =>
            {
                var latestMessageStatus = _twilioService.FetchMessageDeliveryReport(text.SmsId);
                if (latestMessageStatus != null && latestMessageStatus.Result != null && latestMessageStatus.Result.Status != "404")
                {
                    if (text.ErrorMessage == null || text.ErrorMessage == "")
                        text.ErrorMessage = latestMessageStatus.Result.ErrorMessage;
                    if (text.ErrorCode == null || text.ErrorCode == 0)
                        text.ErrorCode = latestMessageStatus.Result.ErrorCode;
                }
            });

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdateFailedDeliveryReportWhatsapp(List<Domain.Entities.DeliveryReportWhatsApp> failedwhatsAppMessage, CancellationToken cancellationToken)
        {
            Parallel.ForEach(failedwhatsAppMessage, text =>
            {
                var latestWhatsAppMessageStatus = _twilioService.FetchMessageDeliveryReport(text.WhatsAppId);
                if (latestWhatsAppMessageStatus != null && latestWhatsAppMessageStatus.Result != null && latestWhatsAppMessageStatus.Result.Status != "404")
                {
                    if (text.ErrorMessage == null || text.ErrorMessage == "")
                        text.ErrorMessage = latestWhatsAppMessageStatus.Result.ErrorMessage;
                    if (text.ErrorCode == null || text.ErrorCode == 0)
                        text.ErrorCode = latestWhatsAppMessageStatus.Result.ErrorCode;
                }
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
