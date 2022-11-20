using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.Managers
{
    public class CustomizationManager : MonoBehaviour
    {
        public static CustomizationManager Instance {get; private set;}
        CustomizationDisplay m_CustomizationDisplay;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            m_CustomizationDisplay = FindObjectOfType<CustomizationDisplay>();
            StartCoroutine(Cor_GetProfileData());
        }
        public void SetCustomization(CustomizationDisplay _display)
        {
            Dictionary<CustomizationType, int> datas = ProfileManager.Instance.ProfileData.CustomizationData.Datas;
            _display.Setup(
                datas[CustomizationType.BODY],
                datas[CustomizationType.BODY_PART],
                datas[CustomizationType.EYES],
                datas[CustomizationType.GLOVES],
                datas[CustomizationType.MOUTH],
                datas[CustomizationType.NOSE],
                datas[CustomizationType.EARS],
                datas[CustomizationType.GLASSES],
                datas[CustomizationType.HAIR],
                datas[CustomizationType.HAT],
                datas[CustomizationType.HORN],
                datas[CustomizationType.TAIL]
            );
        }
        IEnumerator Cor_GetProfileData()
        {
            yield return new WaitUntil(() => m_CustomizationDisplay != null);
            Dictionary<CustomizationType, int> datas = ProfileManager.Instance.ProfileData.CustomizationData.Datas;
            m_CustomizationDisplay.Setup(
                datas[CustomizationType.BODY],
                datas[CustomizationType.BODY_PART],
                datas[CustomizationType.EYES],
                datas[CustomizationType.GLOVES],
                datas[CustomizationType.MOUTH],
                datas[CustomizationType.NOSE],
                datas[CustomizationType.EARS],
                datas[CustomizationType.GLASSES],
                datas[CustomizationType.HAIR],
                datas[CustomizationType.HAT],
                datas[CustomizationType.HORN],
                datas[CustomizationType.TAIL]
            );
        }
    }
}
