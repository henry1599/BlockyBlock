using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using NaughtyAttributes;
using Newtonsoft.Json;

namespace BlockyBlock.Managers
{
    public class ProfileManager : MonoBehaviour
    {
        public static ProfileManager Instance {get; private set;}
        private ProfileData m_ProfileData;
        public ProfileData ProfileData
        {
            get
            {
                if (m_ProfileData == null) return new ProfileData();
                else return m_ProfileData;
            }
        }
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            LoadProfile();
        }
        public void LoadProfile()
        {
            string json = PlayerPrefs.GetString(GameConstants.PROFILE_KEY, string.Empty);
            if (json == string.Empty) 
            {
                m_ProfileData = new ProfileData();
                return;
            }
            m_ProfileData = JsonConvert.DeserializeObject<ProfileData>(json);
        }
        public void SaveProfile()
        {
            string json = JsonConvert.SerializeObject(m_ProfileData);
            Debug.Log("Save String : " + json);
            PlayerPrefs.SetString(GameConstants.PROFILE_KEY, json);
        }
        public void UnlockCustomization(CustomizationType type, int idx)
        {
            // * Validate
            if (!IsValidateCustomization(type, idx))
                return;
            // * Unlock
            // TODO: Validate money here
            //...
            m_ProfileData.customizationData.datas[type].index = idx;
            m_ProfileData.customizationData.datas[type].isUnlock = true;
            CustomizationManager.Instance.LoadCustomization();

            SaveProfile();
        }
        public void ResetCustomization(CustomizationType type)
        {
            int resetIdx = 0;
            switch (type)
            {
                case CustomizationType.BODY:
                case CustomizationType.EYES:
                case CustomizationType.MOUTH:
                    resetIdx = 0;
                    break;
                case CustomizationType.BODY_PART:
                case CustomizationType.EARS:
                case CustomizationType.GLASSES:
                case CustomizationType.GLOVES:
                case CustomizationType.HAIR:
                case CustomizationType.HAT:
                case CustomizationType.HORN:
                case CustomizationType.NOSE:
                case CustomizationType.TAIL:
                default:
                    resetIdx = -1;
                    break;
            }
            
            m_ProfileData.customizationData.datas[type].index = resetIdx;
            CustomizationManager.Instance.LoadCustomization();

            SaveProfile();
        }
        bool IsValidateCustomization(CustomizationType type, int idx)
        {
            if (CustomizationManager.Instance == null)
            {
                Debug.Log("CustomizationManager.Instance is null, cannot validate customization");
                return false;
            }
            CustomizationDisplay display = CustomizationManager.Instance.Display;
            if (display == null)
            {
                Debug.Log("CustomizationManager.Instance.Display is null, cannot validate customization");
                return false;
            }
            int displayCount = display.GetCustomizationCountByType(type);
            return idx >= -1 && idx < displayCount;
        }
        [ExecuteInEditMode]
        [Button("Reset Save")]
        public void ResetSave()
        {
            Debug.Log("Clear PlayerPrefs");
            PlayerPrefs.DeleteAll();
        }
    }
}
