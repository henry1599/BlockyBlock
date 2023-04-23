using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.BackEnd;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class VerificationManager : FormManager
    {
        public UI.VerificationDisplay VerificationDisplay;
        private static readonly string VERIFICATION_MESSAGE = "Verifying your account...";
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
            if (!VerificationDisplay.IsOpenned)
                return;
            this.Timer -= Time.deltaTime;
            this.Timer = Mathf.Max(0, this.Timer);
            if (this.Timer == 0)
            {
                ON_TIMEOUT?.Invoke();
            }
        }
        public void Verify()
        {
            GameEvents.ON_LOADING?.Invoke(true, VERIFICATION_MESSAGE);
            StartCoroutine(Cor_Verify());
        }
        public void Resend()
        {
            GameEvents.ON_LOADING?.Invoke(true, RESEND_MESSAGE);
            StartCoroutine(Cor_Resend());
        }
        void ResetForm()
        {
            this.Timer = WWWManager.Instance.APIConfig.VerificationDuration;
            VerificationDisplay.ResetCountDown();
        }
        IEnumerator Cor_Resend()
        {
            base.isError = false;
            string otpToken = OnlineManager.VERIFICATION_TOKEN_REGISTER;
            ResendRequest resendRequest = new ResendRequest(otpToken);
            WWWManager.Instance.Post(resendRequest, WebType.AUTHENTICATION, APIType.SIGNUP_VERIFICATION_RESEND, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE));
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            ResendResponse verificationResponse = JsonUtility.FromJson<ResendResponse>(resultJson);
            OnlineManager.VERIFICATION_TOKEN_REGISTER = verificationResponse.verifyToken;
            GameEvents.ON_LOADING?.Invoke(false, "");
            Debug.Log("Resend successful ");
            ResetForm();
        }
        IEnumerator Cor_Verify()
        {
            base.isError = false;
            string code = VerificationDisplay.Code;
            string token = OnlineManager.VERIFICATION_TOKEN_REGISTER;
            VerificationRequest verificationRequest = new VerificationRequest(token, code);
            WWWManager.Instance.Post(verificationRequest, WebType.AUTHENTICATION, APIType.USER_VERIFY, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE));
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            VerificationResponse verificationResponse = JsonUtility.FromJson<VerificationResponse>(resultJson);
            // * Do the save token here locally
            Debug.Log("Verify response : " + verificationResponse.ToString());
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
        public class VerificationRequest
        {
            public string token;
            public string code;
            public VerificationRequest(string token, string code)
            {
                this.token = token;
                this.code = code;
            }
            public VerificationRequest()
            {
                this.token = this.code = BEConstants.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class VerificationResponse
        {
            public string code;
            public VerificationResponse(string code)
            {
                this.code = code;
            }
            public VerificationResponse()
            {
                this.code = BEConstants.DEFAULT_VALUE;
            }
        }
    }
}
