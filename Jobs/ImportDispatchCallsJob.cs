﻿using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.DispatchCalls;
using MkeAlerts.Web.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class ImportDispatchCallsJob
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEntityWriteService<DispatchCall, string> _dispatchCallWriteService;

        public ImportDispatchCallsJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEntityWriteService<DispatchCall, string> dispatchCallWriteService)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _dispatchCallWriteService = dispatchCallWriteService;
        }

        public async Task Run()
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(_configuration["JobUserId"]);
            ClaimsPrincipal claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(applicationUser);

            string url = @"https://itmdapps.milwaukee.gov/MPDCallData/index.jsp?district=All";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            foreach (var row in doc.DocumentNode.SelectNodes("//table/tbody/tr"))
            {
                try
                {
                    var cols = row.SelectNodes("td");

                    DispatchCall dispatchCall = await _dispatchCallWriteService.GetOne(claimsPrincipal, cols[0].InnerText);

                    if (dispatchCall == null)
                    {
                        dispatchCall = new DispatchCall()
                        {
                            CallNumber = cols[0].InnerText,
                            DateTime = DateTime.Parse(cols[1].InnerText),
                            Location = cols[2].InnerText,
                            District = int.Parse(cols[3].InnerText),
                            NatureOfCall = cols[4].InnerText,
                            Status = cols[5].InnerText
                        };

                        await _dispatchCallWriteService.Create(claimsPrincipal, dispatchCall);
                    }
                    else
                    {
                        dispatchCall.Status = cols[5].InnerText;

                        await _dispatchCallWriteService.Update(claimsPrincipal, dispatchCall);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}