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
        public CustomizationDisplay Display => m_CustomizationDisplay;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            m_CustomizationDisplay = FindObjectOfType<CustomizationDisplay>();
            LoadCustomization();
        }
        public void SetCustomization(CustomizationDisplay _display)
        {
            Dictionary<CustomizationType, CustomizationStatus> datas = ProfileManager.Instance.ProfileData.customizationData.datas;
            _display.Setup(
                datas[CustomizationType.BODY].index,
                datas[CustomizationType.BODY_PART].index,
                datas[CustomizationType.EYES].index,
                datas[CustomizationType.GLOVES].index,
                datas[CustomizationType.MOUTH].index,
                datas[CustomizationType.NOSE].index,
                datas[CustomizationType.EARS].index,
                datas[CustomizationType.GLASSES].index,
                datas[CustomizationType.HAIR].index,
                datas[CustomizationType.HAT].index,
                datas[CustomizationType.HORN].index,
                datas[CustomizationType.TAIL].index
            );
        }
        public void LoadCustomization()
        {
            StartCoroutine(Cor_GetProfileData());
        }
        IEnumerator Cor_GetProfileData()
        {
            yield return new WaitUntil(() => m_CustomizationDisplay != null);
            Dictionary<CustomizationType, CustomizationStatus> datas = ProfileManager.Instance.ProfileData.customizationData.datas;
            m_CustomizationDisplay.Setup(
                datas[CustomizationType.BODY].index,
                datas[CustomizationType.BODY_PART].index,
                datas[CustomizationType.EYES].index,
                datas[CustomizationType.GLOVES].index,
                datas[CustomizationType.MOUTH].index,
                datas[CustomizationType.NOSE].index,
                datas[CustomizationType.EARS].index,
                datas[CustomizationType.GLASSES].index,
                datas[CustomizationType.HAIR].index,
                datas[CustomizationType.HAT].index,
                datas[CustomizationType.HORN].index,
                datas[CustomizationType.TAIL].index
            );
        }
        public void PlayAnim(string animation)
        {
            Display.GetComponent<Animator>()?.CrossFade(animation, 0, 0);
        }
    }
}
