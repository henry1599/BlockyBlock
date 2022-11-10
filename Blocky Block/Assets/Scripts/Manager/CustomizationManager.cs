using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class CustomizationManager : MonoBehaviour
    {
        CustomizationDisplay[] m_CustomizationDisplays;
        void Start()
        {
            m_CustomizationDisplays = FindObjectsOfType<CustomizationDisplay>();
            StartCoroutine(Cor_GetProfileData());
        }
        IEnumerator Cor_GetProfileData()
        {
            yield return new WaitUntil(() => ProfileManager.Instance != null && m_CustomizationDisplays.Length > 0);
            Dictionary<CustomizationType, int> datas = ProfileManager.Instance.ProfileData.CustomizationData.Datas;
            foreach(CustomizationDisplay display in m_CustomizationDisplays)
            {
                display.Setup(
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
}
