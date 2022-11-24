using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlockyBlock.Managers;

namespace BlockyBlock.UI
{
    public class LoginDisplay : MonoBehaviour
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
    }
}
