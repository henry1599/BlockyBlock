using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using BlockyBlock.BackEnd;
using TMPro;
using DG.Tweening;
using System.Text;

namespace BlockyBlock.UI
{
    public class ErrorDisplay : MonoBehaviour
    {
        [SerializeField] Transform container;
        [SerializeField] TMP_Text errorText;
        [SerializeField] Color titleColor;
        StringBuilder stringBuilder = new StringBuilder();
        void Start()
        {
            WWWManager.ON_ERROR += HandleError;
        }
        void OnDestroy()
        {
            WWWManager.ON_ERROR -= HandleError;
        }
        void HandleError(APIType apiType, string message)
        {
            this.container.DOScaleX(1, 0.35f).SetEase(Ease.InOutSine);
            this.container.DOScaleX(0, 0.35f).SetEase(Ease.InOutSine).SetDelay(3f);
            stringBuilder.Clear();
            string title = APITypeToString(apiType);
            stringBuilder.AppendFormat("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>\n{4}", (byte)(this.titleColor.r * 255f), (byte)(this.titleColor.g * 255f), (byte)(this.titleColor.b * 255f), title, message);
            errorText.text = stringBuilder.ToString();
        }
        string APITypeToString(APIType apiType)
        {
            switch (apiType)
            {
                case APIType.HEALTH_CHECK:
                    return "Health Check Error";
                case APIType.GUEST_LOGIN:
                    return "Guest Login Error";
                case APIType.USER_LOGIN:
                    return "Login Error";
                case APIType.USER_REGISTER:
                    return "Register Error";
                case APIType.USER_VERIFY:
                case APIType.SIGNUP_VERIFICATION_RESEND:
                case APIType.FORGOT_PASSWORD_VERIFICATION_RESEND:
                    return "Verification Error";
                case APIType.LOGOUT:
                case APIType.FORGOT_PASSWORD:
                default:
                    return "Health Check Error";
            }
        }
    }
}
