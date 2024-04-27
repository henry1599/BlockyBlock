using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using BlockyBlock.BackEnd;
using System;

namespace BlockyBlock.Managers
{
    [System.Serializable]
    public class ErrorResponse
    {
        public ErrorData error;
        public ErrorResponse()
        {
            this.error = new ErrorData();
        }
    }
    [System.Serializable]
    public class ErrorData
    {
        public string code;
        public string message;
        public ErrorData(string code, string message)
        {
            this.code = code;
            this.message = message;
        }
        public ErrorData()
        {
            this.code = this.message = "";
        }
    }
    [System.Serializable]
    public class RenewAccessTokenRequest
    {
        public string refreshToken;
        public RenewAccessTokenRequest(string refreshToken)
        {
            this.refreshToken = refreshToken;
        }
        public RenewAccessTokenRequest()
        {
            this.refreshToken = BEConstants.DEFAULT_VALUE;
        }
    }
    [System.Serializable]
    public class RenewAccessTokenResponse
    {
        public string refreshToken;
        public string accessToken;
        public RenewAccessTokenResponse(string refreshToken, string accessToken)
        {
            this.refreshToken = refreshToken;
            this.accessToken = accessToken;
        }
    }
    public class WWWManager : MonoBehaviour
    {
        public static WWWManager Instance {get; private set;}
        public static System.Action<APIType, string> ON_ERROR;
        public APIConfig APIConfig;
        public bool IsComplete {get; set;}
        private bool isErrorSelf = false;
        public string Result 
        {
            get => result;
        }
        string result = "";
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void RenewAccessToken(System.Action callBack = null)
        {
            StartCoroutine(Cor_RenewAccessToken(callBack));
        }
        IEnumerator Cor_RenewAccessToken(System.Action callBack = null)
        {
            var apiType = GameManager.Instance.IsGuest ? APIType.GUEST_RENEW_ACCESS_TOKEN : APIType.USER_RENEW_ACCESS_TOKEN;
            string refreshToken = GameManager.Instance.RefreshToken;
            RenewAccessTokenRequest request = new RenewAccessTokenRequest(refreshToken);
            Post(request, WebType.AUTHENTICATION, apiType, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE));
            yield return new WaitUntil(() => IsComplete);
            if (this.isErrorSelf)
            {
                Debug.LogError("Error when renewing access token, reason: Unknown");
                PlayerPrefs.DeleteKey(BEConstants.ACCESS_TOKEN_KEY);
                PlayerPrefs.DeleteKey(BEConstants.REFRESH_TOKEN_KEY);
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            RenewAccessTokenResponse renewResponse = JsonUtility.FromJson<RenewAccessTokenResponse>(resultJson);
            PlayerPrefs.SetString(BEConstants.ACCESS_TOKEN_KEY, renewResponse.accessToken);
            PlayerPrefs.SetString(BEConstants.REFRESH_TOKEN_KEY, renewResponse.refreshToken);
            GameManager.Instance.UpdateTokens();
            yield return null;
            callBack?.Invoke();
        }
        public void Get(WebType webType, APIType apiType)
        {
#if ENABLE_LOGIN
            IsComplete = true;
#else
            IsComplete = false;
            StartCoroutine(Cor_Get(webType, apiType));
#endif
        }
        public void Post(object objToSend, WebType webType, APIType apiType, params (string, string)[] headers)
        {
#if ENABLE_LOGIN
            IsComplete = true;
#else
            IsComplete = false;
            StartCoroutine(Cor_Post(objToSend, webType, apiType, headers));
#endif
        }
        IEnumerator Cor_Post(object objToSend, WebType webType, APIType apiType, params (string, string)[] headers)
        {
            string json = JsonUtility.ToJson(objToSend);
            string uri = APIConfig.WebData[webType] + APIConfig.APIData[apiType];
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                foreach (var header in headers)
                {
                    webRequest.SetRequestHeader(header.Item1, header.Item2);
                }
                
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                string errorMessage = "";
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        errorMessage = JsonUtility.FromJson<ErrorResponse>(webRequest.downloadHandler.text).error.message;
                        ON_ERROR?.Invoke(apiType, errorMessage);
                        this.isErrorSelf = true;
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        string jsonReceived = webRequest.downloadHandler.text;
                        var errorResponse = JsonUtility.FromJson<ErrorResponse>(jsonReceived);
                        errorMessage = errorResponse.error.message;
                        var code = errorResponse.error.code;
                        Debug.Log("Json Recieved : " + jsonReceived);
                        Debug.Log("Error code : " + code);
                        this.isErrorSelf = true;
                        if (code.Equals("ACCESS_TOKEN_EXPIRED"))
                        {
                            Debug.Log("Access token is expired, renewing...");
                            RenewAccessToken(() => Post(objToSend, webType, apiType, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE), (BEConstants.CONTENT_TYPE_TRACKING, GameManager.Instance.AccessToken)));
                            yield break;
                        }
                        ON_ERROR?.Invoke(apiType, errorMessage);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Post Success, json Posted: " + json);
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        this.result = webRequest.downloadHandler.text;
                        this.isErrorSelf = false;
                        break;
                }

                yield return new WaitForEndOfFrame();
            }
            IsComplete = true;
        }
        IEnumerator Cor_Get(WebType webType, APIType apiType)
        {
            string uri = APIConfig.WebData[webType] + APIConfig.APIData[apiType];
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbGET))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        ON_ERROR?.Invoke(apiType, webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        ON_ERROR?.Invoke(apiType, webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        this.result = webRequest.downloadHandler.text;
                        break;
                }

                yield return new WaitForEndOfFrame();
            }
            IsComplete = true;
        }
    }
}
