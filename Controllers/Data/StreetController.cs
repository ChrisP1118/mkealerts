﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Models.DTO.Places;
using MkeAlerts.Web.Services;

namespace MkeAlerts.Web.Controllers.Data
{
    public class StreetController : EntityReadController<Street, StreetDTO, IEntityReadService<Street, string>, string>
    {
        public StreetController(IConfiguration configuration, IMapper mapper, IEntityReadService<Street, string> service) : base(configuration, mapper, service)
        {
        }
    }
}