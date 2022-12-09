using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.BackEnd;
using BlockyBlock.Managers;

namespace BlockyBlock.UI
{
    public class LoginDisplay : FormDisplay
    {
        [SerializeField] TMP_InputField emailField;
        [SerializeField] TMP_InputField passwordField;
        public string Email 
        {
            get
            {
                if (string.IsNullOrEmpty(emailField.text))
                {
                    return "";
                }
                return emailField.text;
            }
        }
        public string Password
        {
            get 
            {
                if (string.IsNullOrEmpty(passwordField.text))
                {
                    return "";
                }
                return passwordField.text;
            }
        }
        
        public void OnSignupButtonClick()
        {
            BEFormEvents.ON_ENABLED?.Invoke(FormType.SIGNUP_FORM, null);
        }
        public void OnForgotPasswordButtonClick()
        {
            BEFormEvents.ON_ENABLED?.Invoke(FormType.FORGOT_PASSWORD_EMAIL_FORM, null);
        }
    }
}
