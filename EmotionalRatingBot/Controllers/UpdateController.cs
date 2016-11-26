using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmotionalRatingBot.Controllers
{
    using System.Net.Http.Headers;
    using System.Threading;

    using EmotionalRatingBot.Storage;

    public class UpdateController : ApiController
    {
        private const int LONG_TIMEOUT_SEC = 100;

        private const string VERSION_HEADER_NAME = "Version";

        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var task = DataManager.getInstance().IsUpdated(this.ParseVersionHeader(Request.Headers));
            if (task.Wait(LONG_TIMEOUT_SEC * 1000))
            {
                return Request.CreateResponse(HttpStatusCode.OK, task.Result.ToString());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
        }

        private long ParseVersionHeader(HttpRequestHeaders headers)
        {
            long version;
            if (headers.Contains(VERSION_HEADER_NAME)
                && headers.GetValues(VERSION_HEADER_NAME).FirstOrDefault().Length > 0)
            {
                var hval = headers.GetValues(VERSION_HEADER_NAME).FirstOrDefault();
                if (long.TryParse(hval, out version))
                {
                    return version;
                }
            }

            return -1;
        }
    }
}