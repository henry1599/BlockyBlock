using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.BackEnd;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class ForgotPasswordEmailManager : FormManager
    {
        public UI.ForgotPasswordEmailDisplay ForgotPasswordEmailDisplay;
        private static readonly string CONFIRM_MESSAGE = "Sending code to your email...";        
        public override void Start()
        {
            base.Start();
        }
        public void Confirm()
        {
            GameEvents.ON_LOADING?.Invoke(true, CONFIRM_MESSAGE);
            StartCoroutine(Cor_Confirm());
        }
        IEnumerator Cor_Confirm()
        {
            base.isError = false;
            string email = ForgotPasswordEmailDisplay.Email;
            ForgotPasswordRequest forgotPasswordRequest = new ForgotPasswordRequest(email);
            WWWManager.Instance.Post(forgotPasswordRequest, WebType.AUTHENTICATION, APIType.FORGOT_PASSWORD_REQUEST, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE));
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            ForgotPasswordResponse forgotPasswordResponse = JsonUtility.FromJson<ForgotPasswordResponse>(resultJson);
            // * Do the save token here locally
            OnlineManager.FORGOT_PASSWORD_VERIFY_TOKEN = forgotPasswordResponse.verifyToken;
            Debug.Log("Forgot password response : " + forgotPasswordResponse.ToString());
            GameEvents.ON_LOADING?.Invoke(false, "");

            BEFormEvents.ON_ENABLED?.Invoke(FormType.FORGOT_PASSWORD_FORM, () => BEFormEvents.ON_OPEN_FORGOTPASSWORD_FORM?.Invoke(email));
        }
        [System.Serializable]
        public class ForgotPasswordRequest
        {
            public string email;
            public ForgotPasswordRequest(string email)
            {
                this.email = email;
            }
            public ForgotPasswordRequest()
            {
                this.email = BEConstants.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class ForgotPasswordResponse
        {
            public string code;
            public string message;
            public string verifyToken;
            public ForgotPasswordResponse(string code, string message, string verifyToken)
            {
                this.code = code;
                this.message = message;
                this.verifyToken = verifyToken;
            }
            public ForgotPasswordResponse()
            {
                this.code = this.message = this.verifyToken = BEConstants.DEFAULT_VALUE;
            }
        }
    }
}
