namespace FaceAPI
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Azure.CognitiveServices.Vision.Face;
    using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

    public class FaceAPI
    {
        private const string _SUBSCRIPTION_KEY_ = "8e2b634b7c804a7?????????";
        //private const string _FACE_ENDPOINT_ = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";
        private const string _FACE_ENDPOINT_ = "https://westcentralus.api.cognitive.microsoft.com";
        private readonly FaceAttributeType[] _faceAttributes = { FaceAttributeType.Age, FaceAttributeType.Gender };
        private FaceClient _faceClient;

        public delegate void GetFaceAttributesCallback(double? age, Gender? gender);

        public enum Gender
        {
            Male = 0,
            Female = 1,
            Genderless = 2
        }

        public void Init()
        {
            if (_faceClient != null)
            {
                _faceClient.Dispose();
                _faceClient = null;
            }

            _faceClient = new FaceClient(
                new ApiKeyServiceClientCredentials(_SUBSCRIPTION_KEY_),
                new System.Net.Http.DelegatingHandler[] { });
            _faceClient.Endpoint = _FACE_ENDPOINT_;
        }

        public async Task GetFaceAttributesForLocal(string imagePath, GetFaceAttributesCallback callback)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException();

            try
            {
                using (Stream imageStream = File.OpenRead(imagePath))
                {
                    IList<DetectedFace> faceList =
                            await _faceClient.Face.DetectWithStreamAsync(
                                imageStream, true, false, _faceAttributes);

                    if (faceList == null)
                    {
                        throw new Exception("faceList is null");
                    }
                    else if (faceList.Count <= 0)
                    {
                        throw new Exception("No recognized face.");
                    }
                    else
                    {
                        callback(faceList[0].FaceAttributes.Age, (Gender)faceList[0].FaceAttributes.Gender);
                    }
                }
            }
            catch (APIErrorException ex)
            {
                throw ex;
            }
        }

        public async Task GetFaceAttributesForUrl(string imageUrl, GetFaceAttributesCallback callback)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                throw new UriFormatException();

            try
            {
                IList<DetectedFace> faceList =
                            await _faceClient.Face.DetectWithUrlAsync(
                                imageUrl, true, false, _faceAttributes);

                if (faceList == null)
                {
                    throw new Exception("faceList is null");
                }
                else if (faceList.Count <= 0)
                {
                    throw new Exception("No recognized face.");
                }
                else
                {
                    callback(faceList[0].FaceAttributes.Age, (Gender)faceList[0].FaceAttributes.Gender);
                }
            }
            catch (APIErrorException ex)
            {
                throw ex;
            }
        }
    }
}
