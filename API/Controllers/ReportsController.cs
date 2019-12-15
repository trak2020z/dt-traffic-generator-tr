﻿using System;
using API.Models.Report;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TestLibrary.Infrastructure.Common.Const;
using TestLibrary.Infrastructure.ReportInfrastructure.Abstract;
using TestLibrary.Providers.Abstract;

namespace API.Controllers
{
    [Route("testapi/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportProvider _reportProvider;

        public ReportsController(IReportProvider reportProvider)
        {
            _reportProvider = reportProvider;
        }


        /// <summary>
        /// Method to get the list of all endpoints (for specific test) with average execution times.
        /// </summary>
        [ProducesResponseType(200, Type = typeof(GetAverageEndpointsExecutionTimesResponseModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("GetAverageEndpointsExecutionTimes")]
        public ActionResult GetAverageEndpointsExecutionTimes(long testParametersId)
        {
            try
            {
                IGetAverageEndpointsExecutionTimesResponse getAverageEndpointsResponse = _reportProvider.GetAverageEndpointsExecutionTimes(testParametersId);
                return PrepareHttpResponse(getAverageEndpointsResponse);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "ReportsController(GetAverageEndpointsExecutionTimes)(EXCEPTION)");
                return StatusCode(500);
            }
        }

        private ActionResult PrepareHttpResponse(IGetAverageEndpointsExecutionTimesResponse getAverageEndpointsResponse)
        {
            switch (getAverageEndpointsResponse.ResponseResult)
            {
                case ResponseResultEnum.Success:
                    return Ok(new GetAverageEndpointsExecutionTimesResponseModel(getAverageEndpointsResponse.AverageEndpointExecutionsTimes));
                case ResponseResultEnum.NotFound:
                    return StatusCode(404);
                default:
                    return StatusCode(500);
            }
        }


        /// <summary>
        /// Method to get the list of all users with average execution times for specific test and endpoint.
        /// </summary>
        [ProducesResponseType(200, Type = typeof(GetAverageEndpointsExecutionTimesResponseModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("GetUsersEndpointExecutionTimes")]
        public ActionResult GetUsersEndpointExecutionTimes(long testParametersId, long endpointId)
        {
            try
            {
                IGetUsersEndpointExecutionTimesResponse response = _reportProvider.GetUsersEndpointExecutionTimes(testParametersId, endpointId);
                return PrepareHttpResponse(response);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "ReportsController(GetUsersEndpointExecutionTimes)(EXCEPTION)");
                return StatusCode(500);
            }
        }

        private ActionResult PrepareHttpResponse(IGetUsersEndpointExecutionTimesResponse response)
        {
            switch (response.ResponseResult)
            {
                case ResponseResultEnum.Success:
                    return Ok(new GetUsersEnpointExecutionTimesResponseModel(response.UserEndpointExecuteTimes));
                case ResponseResultEnum.NotFound:
                    return StatusCode(404);
                default:
                    return StatusCode(500);
            }
        }
    }
}
