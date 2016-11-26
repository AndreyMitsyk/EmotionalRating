using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using EmotionalRatingBot.Storage;
using EmotionalRatingBot.CognitiveServices;

namespace EmotionalRatingBot
{

    public class DataController : ApiController
    {
        /// <summary>
        /// GET: api/Data
        /// </summary>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            TableStorageProvider tableStorageProvider = new TableStorageProvider();
            var charData = tableStorageProvider.GetChartData();
            var response = Request.CreateResponse(HttpStatusCode.OK, charData);
            return response;
        }
    }
}