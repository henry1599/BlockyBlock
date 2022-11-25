using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using System.Collections.Generic;

namespace BlockyBlock.Managers
{
    public class LoginManager : FormManager
    {
        public UI.LoginDisplay LoginDisplay;
        public void Login()
        {
            base.isError = false;
            string email = LoginDisplay.Email;
            string password = LoginDisplay.Password;
            LoginRequest loginRequest = new LoginRequest(email, password);
            WWWManager.Instance.Post(loginRequest, APIType.LOGIN, true);
            if (base.isError)
                return;
            string resultJson = WWWManager.Instance.Result;
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(resultJson);
            // * Do the save token here locally
            Debug.Log("Login response : " + loginResponse.ToString());
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
            public string accessToken;
            public string refreshToken;
            public LoginResponse(string code, string accessToken, string refreshToken)
            {
                this.code = code;
                this.accessToken = accessToken;
                this.refreshToken = refreshToken;
            }
            public LoginResponse()
            {
                this.code = this.accessToken = this.refreshToken = OnlineManager.DEFAULT_VALUE;
            }
        }
    }
}
