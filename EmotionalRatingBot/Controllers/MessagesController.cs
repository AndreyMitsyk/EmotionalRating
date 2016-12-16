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
            Activity reply = activity.CreateReply("Отправьте ваше селфи для участия в конкурсе.");

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
                        try
                        {
                            var faces = await faceProvider.GetFaces(imageBlob.Uri.AbsoluteUri);

                            if (faces != null)
                            {
                                var emotions = await ImageProcessingService.GetService().Process(imageBlob, faces);

                                reply = activity.CreateReply($"Спасибо за участие! {emotions} Общие результаты доступны по ссылке: http://akvelonrating.azurewebsites.net/");
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
                                reply = activity.CreateReply($"Извините, не удалось распознать лица.");
                            }
                        }
                        catch
                        {
                            reply = activity.CreateReply($"Извините, произошла ошибка. Попробуйте повторить позднее.");
                        }
                    } else
                    {
                        reply = activity.CreateReply($"Я не уверен, что вы выглядите так... Отправьте ваше селфи для участия в конкурсе.");
                    }
                }
                else
                {
                    if (activity.Text.ToLower().Contains("start") || activity.Text.ToLower().Contains("help") || activity.Text.ToLower().Contains("привет") || activity.Text.ToLower().Contains("помощь"))
                    {
                        reply = activity.CreateReply("Привет! Я Akvelon Emotional Rating бот! Я принимаю фото (селфи) и нахожу максимальные эмоции на фотографии. Отправьте свое селфи, чтобы принять участие.");
                    }
                }
            }

            await connector.Conversations.ReplyToActivityAsync(reply);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}