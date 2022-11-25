using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;

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
    public class WWWManager : MonoBehaviour
    {
        public static WWWManager Instance {get; private set;}
        public static event System.Action<APIType, string> ON_ERROR;
        public APIConfig APIConfig;
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
        }
        public void Get(APIType apiType)
        {
            StartCoroutine(Cor_Get(apiType));
        }
        public void Post(object objToSend, APIType apiType, bool isHasContentType)
        {
            StartCoroutine(Cor_Post(objToSend, apiType, isHasContentType));
        }
        IEnumerator Cor_Post(object objToSend, APIType apiType, bool isHasContentType)
        {
            string json = JsonUtility.ToJson(objToSend);
            string uri = OnlineManager.WEB_URL + APIConfig.APIData[apiType];
            using (UnityWebRequest webRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                if (isHasContentType)
                    webRequest.SetRequestHeader(OnlineManager.CONTENT_TYPE, OnlineManager.CONTENT_VALUE);
                
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
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        string jsonRecieved = webRequest.downloadHandler.text;
                        errorMessage = JsonUtility.FromJson<ErrorResponse>(jsonRecieved).error.message;
                        ON_ERROR?.Invoke(apiType, errorMessage);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        this.result = webRequest.downloadHandler.text;
                        break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        IEnumerator Cor_Get(APIType apiType)
        {
            string uri = OnlineManager.WEB_URL + APIConfig.APIData[apiType];
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
        }
    }
}
