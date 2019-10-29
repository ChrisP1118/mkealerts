﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using System.Linq;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services.Data
{
    public class StreetService : EntityWriteService<Street, string>
    {
        public StreetService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<Street> validator, ILogger<EntityWriteService<Street, string>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<Street>> ApplyIdFilter(IQueryable<Street> queryable, string id)
        {
            return queryable.Where(x => x.NEWDIME_ID == id);
        }

        protected override async Task<IQueryable<Street>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<Street> queryable)
        {
            return queryable;
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, Street dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }
    }
}
