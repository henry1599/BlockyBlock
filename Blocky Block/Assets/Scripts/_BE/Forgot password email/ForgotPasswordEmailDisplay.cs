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
    public class ForgotPasswordEmailDisplay : FormDisplay
    {
        [SerializeField] TMP_InputField emailText;
        [SerializeField] UIHomeButton confirmButton;
        public bool IsOpenned = false;
        StringBuilder stringBuilderEmailField = new StringBuilder();
        public string Email
        {
            get
            {
                if (string.IsNullOrEmpty(emailText.text))
                {
                    return "";
                }
                return emailText.text;
            }
        }
        public override void Start()
        {
            base.Start();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        public void OnBackButtonClick()
        {
            BEFormEvents.ON_ENABLED?.Invoke(FormType.LOGIN_FORM, null);
        }
    }
}
