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
    using EmotionalRatingBot.ImageCore;

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
                    BlobStorageProvider blobProvider = new BlobStorageProvider();
                    var imageBlob = blobProvider.SaveImage(photo.ContentUrl, photo.ContentType);
                    // TODO: remove test code---------------------------------------------------
                    FaceProvider faceProvider = new FaceProvider();
                    var faces = await faceProvider.GetFaces(imageBlob.Uri.AbsoluteUri);

                    if (faces != null)
                    {
                        reply = activity.CreateReply($"Thanks for your rating!\n {faces[0].FaceAttributes.Gender}");
                        await ImageProcessingService.GetService().Process(imageBlob);
                        var attachments = new List<Attachment>();
                        attachments.Add(new Attachment()
                        {
                            ContentUrl = imageBlob.Uri.AbsoluteUri,
                            ContentType = photo.ContentType,
                            Name = photo.Name
                        });
                        reply.Attachments = attachments;
                    }
                    else
                    {
                        await imageBlob.DeleteAsync();
                        reply = activity.CreateReply($"Sorry. Faces were not found.");
                    }
                }
                else
                {
                    reply = activity.CreateReply($"Please send your selfie, to participate in the rating.");
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