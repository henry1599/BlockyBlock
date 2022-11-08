using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.UI;
using AudioPlayer;
using BlockyBlock.Utils;

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
            StartCoroutine(Cor_LoadSoundMap());
        }
        void Start()
        {
            StartCoroutine(Cor_PlayBGMusic());
        }
        IEnumerator Cor_LoadSoundMap()
        {
            yield return new WaitUntil(() => SoundManager.Instance != null);
            SoundManager.Instance.LoadSoundMap(SoundType.HOME); 
        }
        IEnumerator Cor_PlayBGMusic()
        {
            yield return new WaitUntil(() => SoundManager.Instance != null && MusicController.Instance != null);
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
            PlayerPrefs.SetInt(GameConstants.CHAPTER_CHOSEN_KEY, _chapter);
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_SELECTION);
                }
            );
        }
    }
}
