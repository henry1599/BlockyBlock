using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.UI;
using AudioPlayer;
using BlockyBlock.Utils;
using BlockyBlock.Tracking;

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
            // StartCoroutine(Cor_LoadSoundMap());
            GameManager.Instance.TransitionOut();
            GameManager.Instance.SetFromState(FromState.Home);
        }
        void Start()
        {
            TrackingActionEvent.ON_GAME_ENTER?.Invoke();
        }
        void OnDestroy()
        {
            audioSource?.Stop();
        }
        public void OnShopButtonClick()
        {
            TrackingManager.Instance.Helper.SessionFinished.shopButtonClickCount++;
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    GameEvents.LOAD_LEVEL?.Invoke(LevelID.SHOP);
                }
            );
        }
        public void OnProfileButtonClick()
        {
            TrackingManager.Instance.Helper.SessionFinished.profileButtonClickCount++;
        }
        public void OnSettingButtonClick()
        {
            TrackingManager.Instance.Helper.SessionFinished.settingButtonClickCount++;
        }
        public void OnCreditButtonClick()
        {
            TrackingManager.Instance.Helper.SessionFinished.creditButtonClickCount++;
        }
        public void OnStartButtonClick()
        {
            TrackingManager.Instance.Helper.SessionFinished.mainGamePlayButtonClickCount++;
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
            TrackingManager.Instance.Helper.SessionFinished.endCause = EndCause.Quit_button.EndCauseToString();
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
