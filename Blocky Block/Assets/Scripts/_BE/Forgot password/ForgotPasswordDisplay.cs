using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Managers;
using BlockyBlock.Enums;
using TMPro;
using System.Text;

namespace BlockyBlock.UI
{
    public class ForgotPasswordDisplay : FormDisplay
    {
        [SerializeField] TMP_Text emailText;
        [SerializeField] TMP_InputField codeField;
        [SerializeField] TMP_InputField newPassField;
        [SerializeField] TMP_InputField confirmPassField;
        [SerializeField] TMP_Text countDownText;
        [SerializeField] UIHomeButton confirmButton;
        [SerializeField] GameObject countDownField;
        [SerializeField] GameObject resendButton;
        public bool IsOpenned = false;
        StringBuilder stringBuilderEmailField = new StringBuilder();
        public string Code
        {
            get
            {
                if (string.IsNullOrEmpty(codeField.text))
                {
                    return "";
                }
                return codeField.text;
            }
        }
        public string NewPass
        {
            get
            {
                if (string.IsNullOrEmpty(newPassField.text))
                {
                    return "";
                }
                return newPassField.text;
            }
        }
        public string ConfirmPass
        {
            get
            {
                if (string.IsNullOrEmpty(confirmPassField.text))
                {
                    return "";
                }
                return confirmPassField.text;
            }
        }
        public override void Start()
        {
            base.Start();
            BEFormEvents.ON_OPEN_FORGOTPASSWORD_FORM += HandleOpen;
            ForgotPasswordManager.ON_TIMEOUT += HandleTimeout;
            ForgotPasswordManager.ON_TIMER_CHANGED += HandleTimeChanged;
            confirmPassField.onValueChanged.AddListener(value => OnConfirmValueChanged(value));
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            BEFormEvents.ON_OPEN_FORGOTPASSWORD_FORM -= HandleOpen;
            ForgotPasswordManager.ON_TIMEOUT -= HandleTimeout;
            ForgotPasswordManager.ON_TIMER_CHANGED -= HandleTimeChanged;
            confirmPassField.onValueChanged.RemoveListener(value => OnConfirmValueChanged(value));
        }
        void OnConfirmValueChanged(string value)
        {
            confirmButton.Interactable = value.Equals(newPassField.text);
        }
        void HandleOpen(string email)
        {
            IsOpenned = true;
            stringBuilderEmailField.Clear();
            stringBuilderEmailField.AppendFormat("Please check your email\nWe have sent a code to {0}", email);
            emailText.text = stringBuilderEmailField.ToString();
        }
        void HandleTimeout()
        {
            resendButton.SetActive(true);
            countDownField.SetActive(false);
        }
        void HandleTimeChanged(float value)
        {
            countDownText.text = ((int)value).ToString();
        }
        public void OnBackButtonClick()
        {
            BEFormEvents.ON_ENABLED?.Invoke(FormType.LOGIN_FORM, null);
        }
        public void ResetCountDown()
        {
            resendButton.SetActive(false);
            countDownField.SetActive(true);
        }
    }
}
