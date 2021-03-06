﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MkeTools.Web.Data;
using MkeTools.Web.Exceptions;
using MkeTools.Web.Filters.Support;
using MkeTools.Web.Middleware.Exceptions;
using MkeTools.Web.Models.Data;
using MkeTools.Web.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MkeTools.Web.Controllers
{
    [ApiController]
    public abstract class RelationshipReadController<TDataModel, TDTOModel, TService> : ControllerBase
        where TDataModel : class
        where TDTOModel : class
        where TService : IRelationshipReadService<TDataModel>
    {
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;
        protected readonly IRelationshipReadService<TDataModel> _readService;

        public RelationshipReadController(IConfiguration configuration, IMapper mapper, TService readService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _readService = readService;
        }

        /// <summary>
        /// Returns all items
        /// </summary>
        /// <param name="offset">The first offset in the result set to return</param>
        /// <param name="limit">The maximum number of results to return</param>
        /// <param name="order">The order in which to sort results</param>
        /// <param name="filter">The filters to apply to the results</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [SwaggerResponseHeader(StatusCodes.Status200OK, "X-Total-Count", "int", "Returns the total number of available items")]
        public async Task<ActionResult<IEnumerable<TDTOModel>>> GetAllAsync(Guid parentId, int offset = 0, int limit = 10, string order = null, string filter = null)
        {
            List<TDataModel> dataModelItems = await _readService.GetAll(HttpContext.User, parentId, offset, limit, order, filter);

            List<TDTOModel> dtoModelItems = dataModelItems
                .Select(d => _mapper.Map<TDataModel, TDTOModel>(d))
                .ToList();

            long count = await _readService.GetAllCount(HttpContext.User, parentId, filter);
            Response.Headers.Add("X-Total-Count", count.ToString());

            return Ok(dtoModelItems);
        }

        /// <summary>
        /// Returns a single item
        /// </summary>
        /// <param name="id">The ID of the item to return</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<TDTOModel>> GetOne(Guid parentId, Guid id)
        {
            TDataModel dataModel = await _readService.GetOne(HttpContext.User, parentId, id);

            if (dataModel == null)
                return NotFound();

            TDTOModel dtoModel = _mapper.Map<TDataModel, TDTOModel>(dataModel);

            return Ok(dtoModel);
        }
    }
}