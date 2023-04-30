using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Managers;
using BlockyBlock.Events;
using BlockyBlock.BackEnd;
using BlockyBlock.Enums;

namespace BlockyBlock.UI
{
    public class VolumeSetting : MonoBehaviour
    {
        public Slider musicSlider;
        public Slider sfxSlider;
        void Start()
        {
            musicSlider.onValueChanged.AddListener(HandleMusicValueChanged);
            sfxSlider.onValueChanged.AddListener(HandleSfxValueChanged);
        }
        void OnDestroy()
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.RemoveAllListeners();
        }
        void OnEnable()
        {
            musicSlider.value = SoundManager.Instance.GetMusicVolume();
            sfxSlider.value = SoundManager.Instance.GetSFXVolume();
        }
        void HandleMusicValueChanged(float value)
        {
            SoundManager.Instance.SetMusicVolume(value);
        }
        void HandleSfxValueChanged(float value)
        {
            SoundManager.Instance.SetSFXVolume(value);
        }
        public void OnLogoutButtonClick()
        {
            PlayerPrefs.DeleteKey(BEConstants.ACCESS_TOKEN_KEY);
            PlayerPrefs.DeleteKey(BEConstants.REFRESH_TOKEN_KEY);
            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.ENTRY));
        }
    }
}
