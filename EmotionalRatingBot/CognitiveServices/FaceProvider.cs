using Microsoft.ProjectOxford.Face;
using System.Threading.Tasks;

namespace EmotionalRatingBot.CognitiveServices
{
    public class FaceProvider
    {
        public async Task<Microsoft.ProjectOxford.Face.Contract.Face[]> GetFaces(string imageUrl)
        {
            // TODO: replace sample API key
            string OxfordAPIKey = "fbc8d74540864a5db8273fdb5c26eed8";

            FaceServiceClient OxFaceRecognizer = new FaceServiceClient(OxfordAPIKey);
            var requiredFaceAttributes = new FaceAttributeType[] {
                FaceAttributeType.Gender
            };
            var faces = await OxFaceRecognizer.DetectAsync(imageUrl, false, false, requiredFaceAttributes);

            if (faces != null && faces.Length > 0)
            {
                return faces;
            }

            return null;
        }
    }
}