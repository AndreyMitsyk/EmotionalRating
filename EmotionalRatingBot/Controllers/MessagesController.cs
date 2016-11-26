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
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            Activity reply = activity.CreateReply("Please send your selfie, to participate in the rating.");

            if (activity.Type == ActivityTypes.Message)
            {
                if (activity.Attachments != null && activity.Attachments.Count > 0)
                {
                    var photo = activity.Attachments[0];
                    if (!photo.ContentType.Contains("webp"))
                    {
                        BlobStorageProvider blobProvider = new BlobStorageProvider();
                        var imageBlob = blobProvider.SaveImage(photo.ContentUrl, photo.ContentType);
                        FaceProvider faceProvider = new FaceProvider();
                        var faces = await faceProvider.GetFaces(imageBlob.Uri.AbsoluteUri);

                        if (faces != null)
                        {
                            reply = activity.CreateReply($"Thanks for your rating! See more results here: http://akvelonrating.azurewebsites.net/");
                            await ImageProcessingService.GetService().Process(imageBlob, faces);
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
                    } else
                    {
                        reply = activity.CreateReply($"I'm not sure, that you look like this... Please send your selfie, to participate in the rating.");
                    }
                }
                else
                {
                    if (activity.Text.ToLower().Contains("start") || activity.Text.ToLower().Contains("help"))
                    {
                        reply = activity.CreateReply("Hello! I'm Akvelon Emotional Rating bot! I am receiving the photo (selfie) and I am making an assessment of the event in accordance with the emotions on selfie. Send your foto, to participate in the rating.");
                    }
                }
            }

            await connector.Conversations.ReplyToActivityAsync(reply);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}