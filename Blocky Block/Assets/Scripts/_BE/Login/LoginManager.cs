using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using System.Collections.Generic;
using BlockyBlock.BackEnd;
using System.Threading.Tasks;
using BlockyBlock.Events;

namespace BlockyBlock.Managers
{
    public class LoginManager : FormManager
    {
        public UI.LoginDisplay LoginDisplay;
        private static readonly string LOGIN_MESSAGE = "Logging in...";
        private static readonly string FORGOTPASSWORD_MESSAGE = "Requesting your action...";
        public void Login()
        {
            GameEvents.ON_LOADING?.Invoke(true, LOGIN_MESSAGE);
            StartCoroutine(Cor_Login());
        }
        public void Guest()
        {
            GameEvents.ON_LOADING?.Invoke(true, LOGIN_MESSAGE);
            StartCoroutine(Cor_GuestLogin());
        }
        public void ForgotPassword()
        {
            string savedEmail = PlayerPrefs.GetString(BEConstants.EMAIL, string.Empty);
            if (savedEmail == string.Empty)
            {
                WWWManager.ON_ERROR?.Invoke(APIType.FORGOT_PASSWORD, "You need to have account to perform this action");
            }
            else
            {
                GameEvents.ON_LOADING?.Invoke(true, FORGOTPASSWORD_MESSAGE);
                StartCoroutine(Cor_ForgotPassword(savedEmail));
            }
        }
        IEnumerator Cor_ForgotPassword(string email)
        {
            base.isError = false;
            ForgotPasswordRequest forgotPasswordRequest = new ForgotPasswordRequest(email);
            WWWManager.Instance.Post(forgotPasswordRequest, WebType.AUTHENTICATION, APIType.FORGOT_PASSWORD_REQUEST, true);
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
        IEnumerator Cor_GuestLogin()
        {
            base.isError = false;
            string systemId = WWWManager.Instance.APIConfig.GetUniqueID();
            GuestLoginRequest guestLoginRequest = new GuestLoginRequest(systemId);
            WWWManager.Instance.Post(guestLoginRequest, WebType.AUTHENTICATION, APIType.GUEST_LOGIN, true);
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            GuestLoginResponse guestLoginResponse = JsonUtility.FromJson<GuestLoginResponse>(resultJson);
            // * Do the save token here locally
            PlayerPrefs.SetString(BEConstants.ACCESS_TOKEN_KEY, guestLoginResponse.accessToken);
            PlayerPrefs.SetString(BEConstants.REFRESH_TOKEN_KEY, guestLoginResponse.refreshToken);
            PlayerPrefs.SetString(BEConstants.EMAIL, string.Empty);
            Debug.Log("Guest Login response : " + guestLoginResponse.ToString());
            GameEvents.ON_LOADING?.Invoke(false, "");

            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.HOME));
        }
        IEnumerator Cor_Login()
        {
            base.isError = false;
            string email = LoginDisplay.Email;
            string password = LoginDisplay.Password;
            LoginRequest loginRequest = new LoginRequest(email, password);
            WWWManager.Instance.Post(loginRequest, WebType.AUTHENTICATION, APIType.USER_LOGIN, true);
            yield return new WaitUntil(() => WWWManager.Instance.IsComplete);
            if (base.isError)
            {
                GameEvents.ON_LOADING?.Invoke(false, "");
                yield break;
            }
            string resultJson = WWWManager.Instance.Result;
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(resultJson);
            // * Do the save token here locally
            PlayerPrefs.SetString(BEConstants.ACCESS_TOKEN_KEY, loginResponse.accessToken);
            PlayerPrefs.SetString(BEConstants.REFRESH_TOKEN_KEY, loginResponse.refreshToken);
            PlayerPrefs.SetString(BEConstants.EMAIL, email);
            Debug.Log("User login response : " + loginResponse.ToString());
            GameEvents.ON_LOADING?.Invoke(false, "");
            
            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.HOME));
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
        [System.Serializable]
        public class GuestLoginRequest
        {
            public string guestAccount;
            public GuestLoginRequest(string guestAccount)
            {
                this.guestAccount = guestAccount;
            }
            public GuestLoginRequest()
            {
                this.guestAccount = BEConstants.DEFAULT_VALUE;
            }
        }        
        [System.Serializable]
        public class GuestLoginResponse
        {
            public string code;
            public string accessToken;
            public string refreshToken;
            public GuestLoginResponse(string code, string accessToken, string refreshToken)
            {
                this.code = code;
                this.accessToken = accessToken;
                this.refreshToken = refreshToken;
            }
            public GuestLoginResponse()
            {
                this.code = this.accessToken = this.refreshToken = BEConstants.DEFAULT_VALUE;
            }
        }
        [System.Serializable]
        public class LoginRequest
        {
            public string email;
            public string password;
            public LoginRequest(string email, string password)
            {
                this.email = email;
                this.password = password;
            }
            public LoginRequest()
            {
                this.email = this.password = BEConstants.DEFAULT_VALUE;
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
                this.code = this.accessToken = this.refreshToken = BEConstants.DEFAULT_VALUE;
            }
        }
    }
}
