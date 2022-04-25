using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WAS.Application.Common.Settings;
using Models = WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;
using System.Threading;
using System.Collections.Generic;
using WAS.Application.Common.Models;
using AutoMapper;

namespace WAS.Functions
{
    public class DeleteSubscription
    {
        private readonly IWasContextAdmin _contextAdmin;
        private readonly IGraphService _userGraphRepository;
        private readonly IMapper _mapper;

        public DeleteSubscription(
            IWasContextAdmin context,
            IGraphService userGraphRepository,
            IMapper mapper
            )
        {
            _contextAdmin = context;
            _userGraphRepository = userGraphRepository;
            _mapper = mapper;
        }

        [FunctionName("DeleteSubscription")]
        public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"C# Timer trigger for DeleteSubscription function executed at: {DateTime.UtcNow}");
            try
            {
                //list of fields which need to update
                List<string> columnListforUpdate = new List<string>() { "EmployeeId", "LocationId", "PostalCode", "DepartmentId", "JobTitle", "EmployeeType", "EmployeeGroup", "CostCenter","Role"};

                //getting all active and inactive subsriptions
                var subscriptions = await _contextAdmin.Subscriptions
                     .ToListAsync(cancellationToken);

                //looping all subscriptions
                if (subscriptions != null)
                {
                    for (int i = 0; i < subscriptions.Count; i++)
                    {
                        var userResult = await _userGraphRepository.GetUserByUPN(subscriptions[i].Upn);
                        if (userResult != null)
                        {
                            if (!userResult.IsSubscriberPresent)
                            {
                                subscriptions[i].DeletedDate = DateTime.UtcNow;
                                subscriptions[i].ModifiedDate = DateTime.UtcNow;
                                subscriptions[i].IsActive = false;
                            }
                            else
                                await updateExistingSubscription(subscriptions[i], userResult.User, columnListforUpdate);
                        }

                    }
                    await _contextAdmin.SaveChangesAsync();
                }


                log.LogInformation($"C# Timer trigger for DeleteSubscription function Completed at : {DateTime.UtcNow}");
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private async Task updateExistingSubscription(Domain.Entities.Subscription currentSubscription, UserDataFromAd adData, List<string> columnListforUpdate)
        {
            string newValue = "";
            bool flag = false;
            var comparer = new ObjectsComparer.Comparer<Domain.Entities.Subscription>();

            var locationData = await _contextAdmin.Locations
                    .SingleOrDefaultAsync(s => s.Name == adData.OfficeLocation);

            if (locationData != null)
                adData.LocationId = locationData.Id;

            var departmentData = await _contextAdmin.Departments
                    .SingleOrDefaultAsync(s => s.Name == adData.DepartmentName);

            if (departmentData != null)
                adData.DepartmentId = departmentData.Id;

            var ADSubscription = _mapper.Map<Domain.Entities.Subscription>(adData);
            comparer.Compare(currentSubscription, ADSubscription, out var differences);

            for (int i = 0; i < columnListforUpdate.Count; i++)
            {
                if (differences.Any(d => d.MemberPath == columnListforUpdate[i]))
                {
                    newValue = differences.AsEnumerable().Where(d => d.MemberPath == columnListforUpdate[i]).Select(x => x.Value2).ElementAt(0);

                    if (newValue != null && newValue != "")
                    {
                        flag = true;
                        switch (columnListforUpdate[i])
                        {
                            case "LocationId":
                                currentSubscription.LocationId = Convert.ToInt32(newValue);
                                break;
                            case "PostalCode":
                                currentSubscription.PostalCode = Convert.ToInt32(newValue);
                                break;
                            case "DepartmentId":
                                currentSubscription.DepartmentId = Convert.ToInt32(newValue);
                                break;
                            case "JobTitle":
                                currentSubscription.JobTitle = newValue;
                                break;
                            case "EmployeeType":
                                currentSubscription.EmployeeType = newValue;
                                break;
                            case "EmployeeId":
                                currentSubscription.EmployeeId = newValue;
                                break;
                            case "EmployeeGroup":
                                currentSubscription.EmployeeGroup = newValue;
                                break;
                            case "CostCenter":
                                currentSubscription.CostCenter = newValue;
                                break;
                            case "Role":
                                currentSubscription.Role = newValue;
                                break;
                        }
                    }
                }
            }
            //updating table only if a change is there
            if (flag)
            {
                currentSubscription.UpdatedOn = DateTime.UtcNow;
                currentSubscription.UpdatedTimeZone = TimeZoneInfo.Local.ToString();
                _contextAdmin.Subscriptions.Attach(currentSubscription).State = EntityState.Modified;

            }

        }

    }
}
