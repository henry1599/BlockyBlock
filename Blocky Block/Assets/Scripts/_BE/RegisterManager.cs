using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using System.Collections.Generic;

namespace BlockyBlock.Managers
{
    public class RegisterManager : MonoBehaviour
    {
        public UI.RegisterDisplay RegisterDisplay;
        public void Register()
        {

        }
        [System.Serializable]
        public class RegisterRequest
        {
            public string username;
            public string password;
            public RegisterRequest(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
            public RegisterRequest()
            {
                this.username = this.password = OnlineManager.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class RegisterResponse
        {
            public string code;
            public string accessToken;
            public string refreshToken;
            public RegisterResponse(string code, string accessToken, string refreshToken)
            {
                this.code = code;
                this.accessToken = accessToken;
                this.refreshToken = refreshToken;
            }
            public RegisterResponse()
            {
                this.code = this.accessToken = this.refreshToken = OnlineManager.DEFAULT_VALUE;
            }
        }
    }
}
