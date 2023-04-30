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
            TrackingManager.Instance.SetShopButtonClickCount();
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
            TrackingManager.Instance.SetProfileButtonClickCount();
        }
        public void OnSettingButtonClick()
        {
            TrackingManager.Instance.SetSettingButtonClickCount();
            State = HomeState.SETTING;
        }
        public void OnCreditButtonClick()
        {
            TrackingManager.Instance.SetCreditButtonClickCount();
            State = HomeState.CREDIT;
        }
        public void OnStartButtonClick()
        {
            TrackingManager.Instance.SetMainGamePlayButtonClickCount();
            // * Call Event to load Level
            // GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_TEST_3D);
            State = HomeState.LEVEL_TYPE_SELECTION;
        }
        public void OnBackButtonClick()
        {
            if (State == HomeState.LEVEL_TYPE_SELECTION || State == HomeState.CREDIT || State == HomeState.SETTING)
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
            if (TrackingManager.Instance.Helper.SessionFinished == null)
                return;
            TrackingManager.Instance.SetIsProgress(false);
            TrackingManager.Instance.Helper.SessionFinished.entry = LevelEntry.None.LevelEntryToString();
            GameManager.Instance.StopRecordTimeSpent();
            TrackingManager.Instance.Helper.SessionFinished.endCause = EndCause.Quit_button.EndCauseToString();
            TrackingActionEvent.ON_GAME_EXIT?.Invoke();
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
