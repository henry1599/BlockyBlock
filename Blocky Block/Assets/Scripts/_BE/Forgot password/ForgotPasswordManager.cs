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
        public float TimerValue;
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
            this.Timer = TimerValue;
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
        public void Resend()
        {
            GameEvents.ON_LOADING?.Invoke(true, RESEND_MESSAGE);
            StartCoroutine(Cor_Resend());
        }
        void ResetForm()
        {
            this.Timer = TimerValue;
            ForgotPasswordDisplay.ResetCountDown();
        }
        IEnumerator Cor_Resend()
        {
            base.isError = false;
            string otpToken = OnlineManager.VERIFICATION_TOKEN_REGISTER;
            ResendRequest resendRequest = new ResendRequest(otpToken);
            WWWManager.Instance.Post(resendRequest, WebType.AUTHENTICATION, APIType.FORGOT_PASSWORD_VERIFICATION_RESEND, true);
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            GameEvents.ON_LOADING?.Invoke(false, "");
            Debug.Log("Resend successful ");
            ResetForm();
        }
        IEnumerator Cor_Confirm()
        {
            base.isError = false;
            string code = ForgotPasswordDisplay.Code;
            ForgotPasswordRequest forgotPasswordRequest = new ForgotPasswordRequest(OnlineManager.VERIFICATION_TOKEN_REGISTER, code);
            WWWManager.Instance.Post(forgotPasswordRequest, WebType.AUTHENTICATION, APIType.FORGOT_PASSWORD, true);
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            ForgotPasswordResponse forgotPasswordResponse = JsonUtility.FromJson<ForgotPasswordResponse>(resultJson);
            // * Do the save token here locally
            Debug.Log("Verify response : " + forgotPasswordResponse.ToString());
            GameEvents.ON_LOADING?.Invoke(false, "");
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
        public class ForgotPasswordRequest
        {
            public string token;
            public string code;
            public ForgotPasswordRequest(string token, string code)
            {
                this.token = token;
                this.code = code;
            }
            public ForgotPasswordRequest()
            {
                this.token = this.code = BEConstants.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class ForgotPasswordResponse
        {
            public string code;
            public string accessToken;
            public string refreshToken;
            public ForgotPasswordResponse(string code, string accessToken, string refreshToken)
            {
                this.code = code;
                this.accessToken = accessToken;
                this.refreshToken = refreshToken;
            }
            public ForgotPasswordResponse()
            {
                this.code = this.accessToken = this.refreshToken = BEConstants.DEFAULT_VALUE;
            }
        }
    }
}
