using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.ProjectOxford.Emotion;

namespace EmotionalRatingBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity reply;

                if (activity.Attachments.Count > 0)
                {
                    var photo = activity.Attachments[0];
                    var emotions = await this.GetEmotions("https://i.gyazo.com/175456ab2f6c3862666abcd31f3656ce.jpg");//(photo.ContentUrl);

                    // TODO: add url with a results
                    reply = activity.CreateReply($"Thanks for your rating!\n {emotions.Scores.Happiness}");
                    var attachments = new List<Attachment>();

                    // TODO: return result photo
                    attachments.Add(new Attachment()
                    {
                        ContentUrl = photo.ContentUrl,
                        ContentType = photo.ContentType,
                        Name = photo.Name
                    });
                    reply.Attachments = attachments;
                }
                else
                {
                    reply = activity.CreateReply($"To participate in the rating send your selfie.");
                }

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Microsoft.ProjectOxford.Emotion.Contract.Emotion> GetEmotions(string imageUrl)
        {
            // TODO: replace sample API key
            string OxfordAPIKey = "2cabd9f1b2014a04bc04782b3c703539";
            EmotionServiceClient Oxford = new EmotionServiceClient(OxfordAPIKey);

            var results = await Oxford.RecognizeAsync(imageUrl);

            if (results != null && results.Length > 0)
            {
                var emotions = results[0];
                return emotions;
            }
            return null;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}