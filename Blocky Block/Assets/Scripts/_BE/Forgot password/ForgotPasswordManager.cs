using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.BackEnd;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class ForgotPasswordManager : FormManager
    {
        public UI.ForgotPasswordDisplay ForgotPasswordDisplay;
        private static readonly string CONFIRM_MESSAGE = "Changing your password...";
        private static readonly string RESEND_MESSAGE = "Resending your code...";
        public static event System.Action<float> ON_TIMER_CHANGED;
        public static event System.Action ON_TIMEOUT;
        float Timer 
        {
            get => timer;
            set
            {
                timer = value;
                ON_TIMER_CHANGED?.Invoke(value);
            }
        } float timer;
        public override void Start()
        {
            base.Start();
            this.Timer = WWWManager.Instance.APIConfig.VerificationDuration;
        }
        private void Update() 
        {
            if (!ForgotPasswordDisplay.IsOpenned)
                return;
            this.Timer -= Time.deltaTime;
            this.Timer = Mathf.Max(0, this.Timer);
            if (this.Timer == 0)
            {
                ON_TIMEOUT?.Invoke();
            }
        }
        public void Confirm()
        {
            GameEvents.ON_LOADING?.Invoke(true, CONFIRM_MESSAGE);
            StartCoroutine(Cor_Confirm());
        }
        void ResetForm()
        {
            this.Timer = WWWManager.Instance.APIConfig.VerificationDuration;
            ForgotPasswordDisplay.ResetCountDown();
        }
        public void Resend()
        {
            GameEvents.ON_LOADING?.Invoke(true, RESEND_MESSAGE);
            StartCoroutine(Cor_Resend());
        }
        IEnumerator Cor_Resend()
        {
            base.isError = false;
            string otpToken = OnlineManager.FORGOT_PASSWORD_VERIFY_TOKEN;
            ResendRequest resendRequest = new ResendRequest(otpToken);
            WWWManager.Instance.Post(resendRequest, WebType.AUTHENTICATION, APIType.FORGOT_PASSWORD_VERIFICATION_RESEND, true);
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            ResendResponse verificationResponse = JsonUtility.FromJson<ResendResponse>(resultJson);
            OnlineManager.FORGOT_PASSWORD_VERIFY_TOKEN = verificationResponse.verifyToken;
            GameEvents.ON_LOADING?.Invoke(false, "");
            Debug.Log("Resend successful ");
            ResetForm();
        }
        IEnumerator Cor_Confirm()
        {
            base.isError = false;
            string token = OnlineManager.FORGOT_PASSWORD_VERIFY_TOKEN;
            string code = ForgotPasswordDisplay.Code;
            string newPassword = ForgotPasswordDisplay.NewPass;
            ForgotPasswordConfirmRequest forgotPasswordConfirmRequest = new ForgotPasswordConfirmRequest(token, code, newPassword);
            WWWManager.Instance.Post(forgotPasswordConfirmRequest, WebType.AUTHENTICATION, APIType.FORGOT_PASSWORD, true);
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            GameEvents.ON_LOADING?.Invoke(false, "");
            Debug.Log("Change password successfully");

            BEFormEvents.ON_ENABLED?.Invoke(FormType.LOGIN_FORM, null);
        }
        [System.Serializable]
        public class ResendRequest
        {
            public string otpToken;
            public ResendRequest(string otpToken)
            {
                this.otpToken = otpToken;
            }
            public ResendRequest()
            {
                this.otpToken = BEConstants.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class ResendResponse
        {
            public string code;
            public string message;
            public string verifyToken;
            public ResendResponse(string code, string message, string verifyToken)
            {
                this.code = code;
                this.message = message;
                this.verifyToken = verifyToken;
            }
        }
        [System.Serializable]
        public class ForgotPasswordConfirmRequest
        {
            public string token;
            public string code;
            public string password;
            public ForgotPasswordConfirmRequest(string token, string code, string password)
            {
                this.token = token;
                this.code = code;
                this.password = password;
            }
            public ForgotPasswordConfirmRequest()
            {
                this.code = this.token = this.password = BEConstants.DEFAULT_VALUE;
            }
        }
        
    }
}
