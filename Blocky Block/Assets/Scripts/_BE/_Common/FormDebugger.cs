using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BlockyBlock.Cheat
{
    public class FormDebugger : MonoBehaviour
    {
        public Transform loginForm;
        public Transform signupForm;
        public Transform forgotPasswordForm;
        public Transform verificationSignupForm;

        [Button("Reset to play mode")]
        public void ResetToPlayMode()
        {
            ShowLoginForm();
        }

        [Button("Show Login form")]
        public void ShowLoginForm()
        {
            loginForm.localScale = Vector3.one;
            signupForm.localScale = Vector3.zero;
            forgotPasswordForm.localScale = Vector3.zero;
            verificationSignupForm.localScale = Vector3.zero;
        }
        
        [Button("Show Signup form")]
        public void ShowSignupForm()
        {
            loginForm.localScale = Vector3.zero;
            signupForm.localScale = Vector3.one;
            forgotPasswordForm.localScale = Vector3.zero;
            verificationSignupForm.localScale = Vector3.zero;
        }
        
        [Button("Show Forgot password form")]
        public void ShowForgotPasswordForm()
        {
            loginForm.localScale = Vector3.zero;
            signupForm.localScale = Vector3.zero;
            forgotPasswordForm.localScale = Vector3.one;
            verificationSignupForm.localScale = Vector3.zero;
        }

        [Button("Show Verification form")]
        public void ShowVerificationForm()
        {
            loginForm.localScale = Vector3.zero;
            signupForm.localScale = Vector3.zero;
            forgotPasswordForm.localScale = Vector3.zero;
            verificationSignupForm.localScale = Vector3.one;
        }
    }
}
