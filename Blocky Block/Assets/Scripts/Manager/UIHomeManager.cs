using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.UI;
using AudioPlayer;

namespace BlockyBlock.Managers
{
    public class UIHomeManager : MonoBehaviour
    {
        private AudioSource audioSource;
        public HomeState State 
        {
            get => m_State;
            set
            {
                m_State = value;
                HomeEvents.ON_STAGE_CHANGED?.Invoke(value);
            }
        } [SerializeField] HomeState m_State;
        void Awake()
        {
            SoundManager.Instance.LoadSoundMap(SoundType.HOME);    
        }
        void Start()
        {
            MusicController.Instance.PlayMusic(SoundID.HOME_BG_MUSIC);
        }
        void OnDestroy()
        {
            audioSource?.Stop();
        }
        public void OnStartButtonClick()
        {
            // * Call Event to load Level
            // GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_TEST_3D);
            State = HomeState.LEVEL_TYPE_SELECTION;
        }
        public void OnBackButtonClick()
        {
            if (State == HomeState.LEVEL_TYPE_SELECTION)
            {
                State = HomeState.MAIN;
            }
            else if (State == HomeState.CHAPTER_SELECTION)
            {
                State = HomeState.LEVEL_TYPE_SELECTION;
            }
        }
        public void OnExitButtonClick()
        {
            Application.Quit();
        }
        public void OnMainStoryButtonClick()
        {
            State = HomeState.CHAPTER_SELECTION;
        }
        public void OnChapterButtonClick(int _chapter)
        {
            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_MANNUAL_00));
        }
    }
}
