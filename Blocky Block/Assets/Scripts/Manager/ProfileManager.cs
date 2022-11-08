using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using NaughtyAttributes;

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
            m_ProfileData = JsonUtility.FromJson<ProfileData>(json);
        }
        public void SaveProfile()
        {
            string json = JsonUtility.ToJson(m_ProfileData);
            PlayerPrefs.SetString(GameConstants.PROFILE_KEY, json);
        }
        [Button("Reset Save")]
        public void ResetSave()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
