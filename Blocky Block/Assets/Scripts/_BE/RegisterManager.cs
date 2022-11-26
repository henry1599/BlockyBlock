using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using System.Collections.Generic;

namespace BlockyBlock.Managers
{
    public class RegisterManager : FormManager
    {
        public UI.RegisterDisplay RegisterDisplay;
        public void Register()
        {
            base.isError = false;
            string email = RegisterDisplay.Email;
            string password = RegisterDisplay.Password;
            RegisterRequest registerRequest = new RegisterRequest(email, password);
            WWWManager.Instance.Post(registerRequest, APIType.REGISTER, true);
            if (base.isError)
                return;
            string resultJson = WWWManager.Instance.Result;
            RegisterResponse registerResponse = JsonUtility.FromJson<RegisterResponse>(resultJson);
            // * Do the save token here locally
            Debug.Log("Register response : " + registerResponse.ToString());
            // * Open Verification form
        }
        [System.Serializable]
        public class RegisterRequest
        {
            public string email;
            public string password;
            public RegisterRequest(string email, string password)
            {
                this.email = email;
                this.password = password;
            }
            public RegisterRequest()
            {
                this.email = this.password = OnlineManager.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class RegisterResponse
        {
            public string code;
            public string linkAccountToken;
            public RegisterResponse(string code, string linkAccountToken)
            {
                this.code = code;
                this.linkAccountToken = linkAccountToken;
            }
            public RegisterResponse()
            {
                this.code = this.linkAccountToken = OnlineManager.DEFAULT_VALUE;
            }
        }
    }
}
