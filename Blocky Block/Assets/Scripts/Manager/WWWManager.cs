using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using BlockyBlock.BackEnd;

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
        public static System.Action<APIType, string> ON_ERROR;
        public APIConfig APIConfig;
        public bool IsComplete {get; set;}
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
        public void Get(WebType webType, APIType apiType)
        {
            IsComplete = false;
            StartCoroutine(Cor_Get(webType, apiType));
        }
        public void Post(object objToSend, WebType webType, APIType apiType, params (string, string)[] headers)
        {
            IsComplete = false;
            StartCoroutine(Cor_Post(objToSend, webType, apiType, headers));
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
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        string jsonReceived = webRequest.downloadHandler.text;
                        errorMessage = JsonUtility.FromJson<ErrorResponse>(jsonReceived).error.message;
                        Debug.Log("Json Recieved : " + jsonReceived);
                        ON_ERROR?.Invoke(apiType, errorMessage);
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
