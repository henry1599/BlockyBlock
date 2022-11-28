using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using System.Collections.Generic;
using BlockyBlock.BackEnd;
using BlockyBlock.Events;

namespace BlockyBlock.Managers
{
    public class RegisterManager : FormManager
    {
        public UI.RegisterDisplay RegisterDisplay;
        private static readonly string SIGNUP_MESSAGE = "Signing up...";
        public void Register()
        {
            GameEvents.ON_LOADING?.Invoke(true, SIGNUP_MESSAGE);
            StartCoroutine(Cor_Register());
        }
        IEnumerator Cor_Register()
        {
            base.isError = false;
            string email = RegisterDisplay.Email;
            string password = RegisterDisplay.Password;
            RegisterRequest registerRequest = new RegisterRequest(email, password);
            WWWManager.Instance.Post(registerRequest, WebType.AUTHENTICATION, APIType.USER_REGISTER, true);
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            RegisterResponse registerResponse = JsonUtility.FromJson<RegisterResponse>(resultJson);
            // * Do the save token here locally
            OnlineManager.VERIFICATION_TOKEN_REGISTER = registerResponse.verifyToken;
            OnlineManager.REGISTER_EMAIL = email;
            Debug.Log("VerifyToken receive : " + OnlineManager.VERIFICATION_TOKEN_REGISTER);
            Debug.Log("Register response : " + registerResponse.ToString());
            BEFormEvents.ON_ENABLED?.Invoke(FormType.VERIFICATION_FORM, () => BEFormEvents.ON_OPEN_VERIFICATION_FORM?.Invoke());
            
            GameEvents.ON_LOADING?.Invoke(false, "");
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
                this.email = this.password = BEConstants.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class RegisterResponse
        {
            public string code;
            public string message;
            public string verifyToken;
            public RegisterResponse(string code, string message, string verifyToken)
            {
                this.code = code;
                this.verifyToken = verifyToken;
                this.message = message;
            }
            public RegisterResponse()
            {
                this.code = this.verifyToken = this.message = BEConstants.DEFAULT_VALUE;
            }
        }
    }
}