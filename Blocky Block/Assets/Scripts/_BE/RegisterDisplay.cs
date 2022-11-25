using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using TMPro;

namespace BlockyBlock.UI
{
    public class RegisterDisplay : FormDisplay
    {
        [SerializeField] TMP_InputField emailField;
        [SerializeField] TMP_InputField passwordField;
        [SerializeField] TMP_InputField confirmPasswordField;
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
        public string ConfirmPassword
        {
            get 
            {
                if (string.IsNullOrEmpty(confirmPasswordField.text))
                {
                    return "";
                }
                return confirmPasswordField.text;
            }
        }
        
        public void OnBackButtonClick()
        {
            BEFormEvents.ON_ENABLED?.Invoke(FormType.LOGIN_FORM);
        }
    }
}
