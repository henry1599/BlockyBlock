using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace BlockyBlock.Managers
{
    public class LoginManager : MonoBehaviour
    {
        public UI.LoginDisplay LoginDisplay;
        public void Login()
        {
            string email = LoginDisplay.Email;
            string password = LoginDisplay.Password;
            StartCoroutine(Cor_PostRequest(email, password));
            // StartCoroutine(Cor_HealthCheck());
        }
        IEnumerator Cor_HealthCheck()
        {
            string uri = OnlineManager.WEB_URL + OnlineManager.HEALTH_CHECK_API;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                // webRequest. = "application/json";
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;
                }
            }
        }
        IEnumerator Cor_PostRequest(string email, string password)
        {
            LoginRequest loginRequest = new LoginRequest(email, password);
            string json = JsonUtility.ToJson(loginRequest);
            string uri = OnlineManager.WEB_URL + OnlineManager.LOGIN_API;
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                webRequest.SetRequestHeader(OnlineManager.CONTENT_TYPE, OnlineManager.CONTENT_VALUE);
                // Request and wait for the desired page.
                // webRequest. = "application/json";
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;
                }
            }
        }
        [System.Serializable]
        public class LoginRequest
        {
            public string username;
            public string password;
            public LoginRequest(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
            public LoginRequest()
            {
                this.username = this.password = OnlineManager.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class LoginResponse
        {
            public string code;
            public string verifyToken;
            public string accessToken;
            public string refreshToken;
            public string message;
            public LoginResponse(string code, string verifyToken, string accessToken, string refreshToken, string message)
            {
                this.code = code;
                this.verifyToken = verifyToken;
                this.accessToken = accessToken;
                this.refreshToken = refreshToken;
                this.message = message;
            }
            public LoginResponse()
            {
                this.code = this.verifyToken = this.accessToken = this.refreshToken = this.message = OnlineManager.DEFAULT_VALUE;
            }
        }
    }
}
