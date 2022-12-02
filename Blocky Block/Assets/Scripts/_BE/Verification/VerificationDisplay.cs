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
    public class VerificationDisplay : FormDisplay
    {
        [SerializeField] TMP_Text emailText;
        [SerializeField] TMP_Text countDownText;
        [SerializeField] GameObject countDownField;
        [SerializeField] GameObject resendButton;
        [SerializeField] TMP_InputField codeField;
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
        public override void Start()
        {
            base.Start();
            BEFormEvents.ON_OPEN_VERIFICATION_FORM += HandleOpen;
            VerificationManager.ON_TIMEOUT += HandleTimeout;
            VerificationManager.ON_TIMER_CHANGED += HandleTimeChanged;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            BEFormEvents.ON_OPEN_VERIFICATION_FORM -= HandleOpen;
            VerificationManager.ON_TIMEOUT -= HandleTimeout;
            VerificationManager.ON_TIMER_CHANGED -= HandleTimeChanged;
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
