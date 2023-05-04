using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.SceneManagement;
using BlockyBlock.UI;
using AudioPlayer;
using BlockyBlock.Tracking;
using BlockyBlock.BackEnd;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BlockyBlock.Managers
{
    [System.Flags]
    public enum FromState
    {
        Home = 1 << 0,
        Level_selection = 1 << 1,
        Previous_level = 1 << 2,
        Replay_current_level = 1 << 3,
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}
        bool isCheat;
        [SerializeField] GameObject sceneTransition;
        [SerializeField] GameObject cheatMenu, console; 
        [SerializeField] TMPro.TMP_Text gameVersion;

        [SerializeField] LayerMask defaultLayer;
        [SerializeField] LayerMask seebehindLayer;
        private FromState fromState;
        public RuntimeAnimatorController HomeAnim;
        public RuntimeAnimatorController LevelSelectionAnim;
        public RuntimeAnimatorController LevelAnim;
        public AudioSource AudioSource {get; set;}
        public LevelID PreviousLevelID {get; set;}
        private int startTimeSpent = 0;
        public string AccessToken => this.accessToken;
        private string accessToken;
        public string RefreshToken => this.refreshToken;
        private string refreshToken;
        public bool IsGuest => this.isGuest;
        private bool isGuest;
        public bool CanLoadHome => this.canLoadHome;
        private bool canLoadHome;
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
#if ENABLE_CHEAT
            this.isCheat = true;
#else
            this.isCheat = false;
#endif
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendFormat("Version {0} - {1}", Application.version, this.isCheat ? "Cheat" : "Release");
            gameVersion.text = stringBuilder.ToString();
            this.console.SetActive(this.isCheat);
            SceneTransition sceneTransitionInstance = FindObjectOfType<SceneTransition>();
            if (sceneTransitionInstance == null)
            {
                Instantiate(this.sceneTransition, transform);
            }
            UpdateTokens();
        }
        public void SetFromState(params FromState[] froms)
        {
            foreach (var from in froms)
            {
                this.fromState |= from;
            }
        }
        public void UnsetFromState(params FromState[] froms)
        {
            foreach (var from in froms)
            {
                this.fromState &= ~from;
            }
        }
        public bool IsFrom(FromState stateToCheck)
        {
            return this.fromState.HasFlag(stateToCheck);
        }
        public void UpdateTokens()
        {
            this.accessToken = PlayerPrefs.GetString(BEConstants.ACCESS_TOKEN_KEY, string.Empty);
            this.refreshToken = PlayerPrefs.GetString(BEConstants.REFRESH_TOKEN_KEY, string.Empty);
            this.canLoadHome = !string.IsNullOrEmpty(this.accessToken) && !string.IsNullOrEmpty(this.refreshToken);
            this.isGuest = PlayerPrefs.GetInt(BEConstants.GUESS, 1) == 0 ? false : true;
        }
        // Start is called before the first frame update
        void Start()
        {
            GameEvents.ON_WIN += HandleWin;
            GameEvents.ON_LOSE += HandleLose;
            GameEvents.LOAD_LEVEL += HandleLoadLevel;

            SoundManager.ON_FINISH_LOADING_SOUNDMAP += HandleFinishLoadingSoundmap;
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += ModeChangedInEditor;
#endif
        }
        void Update()
        {
            if (this.isCheat)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    bool status = this.cheatMenu.activeSelf;
                    this.cheatMenu.SetActive(!status);
                }
            }
        }
        void StartRecordTimeSpent()
        {
            this.startTimeSpent = (int)Time.time;
        }
        public void StopRecordTimeSpent()
        {
            TrackingManager.Instance.Helper.SessionFinished.timeSpent = (int)Time.time - this.startTimeSpent;
        }
        private void OnApplicationPause(bool pauseStatus) 
        {
            if (pauseStatus)
            {  
                if (TrackingManager.Instance.Helper.SessionFinished == null)
                    return;
                TrackingManager.Instance.SetIsProgress(true);
                TrackingManager.Instance.Helper.SessionFinished.entry = LevelEntry.None.LevelEntryToString();
                StopRecordTimeSpent();
                TrackingActionEvent.ON_GAME_EXIT?.Invoke();
            }    
        }
        void OnApplicationQuit()
        {
            if (TrackingManager.Instance.Helper.SessionFinished == null)
                    return;
            TrackingManager.Instance.SetIsProgress(false);
            TrackingManager.Instance.Helper.SessionFinished.entry = LevelEntry.None.LevelEntryToString();
            StopRecordTimeSpent();
            TrackingActionEvent.ON_GAME_EXIT?.Invoke();
        }
        void HandleFinishLoadingSoundmap()
        {
            // AudioSource = SoundManager.Instance.PlayMusic(SoundID.HOME_BG_MUSIC);
        }
        void OnDestroy()
        {
            GameEvents.ON_WIN -= HandleWin;
            GameEvents.ON_LOSE -= HandleLose;
            GameEvents.LOAD_LEVEL -= HandleLoadLevel;

            SoundManager.ON_FINISH_LOADING_SOUNDMAP -= HandleFinishLoadingSoundmap;
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= ModeChangedInEditor;
#endif
        }

#if UNITY_EDITOR
        private void ModeChangedInEditor(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                if (TrackingManager.Instance.Helper.SessionFinished == null)
                    return;
                TrackingManager.Instance.SetIsProgress(false);
                TrackingManager.Instance.Helper.SessionFinished.entry = LevelEntry.None.LevelEntryToString();
                GameManager.Instance.StopRecordTimeSpent();
                TrackingManager.Instance.Helper.SessionFinished.endCause = EndCause.Quit_button.EndCauseToString();
                TrackingActionEvent.ON_GAME_EXIT?.Invoke();
            }
        }
#endif

        void HandleWin()
        {
            LevelCheckerManager.Instance.ConfirmBlockUsed();
            LevelCheckerManager.Instance.ConfirmStepPassed();

            bool winGame = true;
            bool useBlockPassed = LevelCheckerManager.Instance.BlockUsed <= LevelManager.Instance.CurrentLevelData.MinimumExecutionBlock;
            bool stepPassed = LevelCheckerManager.Instance.StepPassed <= LevelManager.Instance.CurrentLevelData.MinimumExecutionStep;

            StartCoroutine(Cor_ShowVictoryScreen(winGame, useBlockPassed, stepPassed));
            Debug.Log("Win");
        }
        void HandleLose()
        {
            Debug.Log("Lose");
        }
        IEnumerator Cor_ShowVictoryScreen(bool winLevel, bool useBlockPassed, bool stepPassed)
        {
            yield return new WaitUntil(() => VictoryScreenManager.Instance != null);
            yield return Helpers.Helper.GetWait(0.5f);
            VictoryScreenManager.Instance.Show(winLevel, useBlockPassed, stepPassed);
        }
        public void TransitionIn(System.Action _cb = null)
        {
            StartCoroutine(Cor_TransitionIn(_cb));
        }
        public void TransitionOut(System.Action _cb = null)
        {
            StartCoroutine(Cor_TransitionOut(_cb));
        }
        IEnumerator Cor_TransitionIn(System.Action _cb = null)
        {
            yield return new WaitUntil(() => SceneTransition.Instance != null);
            SceneTransition.Instance.TransitionIn();
            yield return new WaitForSeconds(GameConstants.TRANSITION_IN_DURATION);
            _cb?.Invoke();
        }
        IEnumerator Cor_TransitionOut(System.Action _cb = null)
        {
            yield return new WaitUntil(() => SceneTransition.Instance != null);
            SceneTransition.Instance.TransitionOut();
            yield return new WaitForSeconds(GameConstants.TRANSITION_OUT_DURATION);
            _cb?.Invoke();
        }
        void HandleLoadLevel(LevelID _id)
        {
            StartCoroutine(Cor_UpdateSceneID(_id));
        }
        IEnumerator Cor_UpdateSceneID(LevelID _id)
        {
            PreviousLevelID = LevelManager.Instance.CurrentLevelID;
            yield return new WaitUntil(() => ConfigManager.Instance != null);
            LevelManager.Instance.CurrentLevelID = _id;
            LevelManager.Instance.ResetChecker();
            StartRecordForLevel(_id);
            PlayBGMusic(_id);
        }
        void StartRecordForLevel(LevelID _id)
        {
            int iid = (int)_id;
            if (1000 <= iid && iid <= 2000)
            {
                TrackingManager.Instance.StartRecord(Tracking.RecordDataType.LEVEL_TRIGGER);
                TrackingManager.Instance.StartRecord(Tracking.RecordDataType.LEVEL_FINISHED);

                int levelIdBase = iid % 1000 + 1;
                LevelManager.Instance.SetLevelPlaysInSession(levelIdBase);

                LevelManager.Instance.SetLevelIDLevelTrigger();
                LevelManager.Instance.SetChapterIDLevelTrigger();
                LevelManager.Instance.SetEntryLevelTrigger();
                TrackingManager.Instance.StopRecord(Tracking.RecordDataType.LEVEL_TRIGGER);

                LevelManager.Instance.SetLevelIDLevelFinished();
                LevelManager.Instance.SetChapterIDLevelFinished();
                LevelManager.Instance.SetEntryLevelFinished();
                LevelManager.Instance.StartTimer();
            }
        }
        void PlayBGMusic(LevelID _id)
        {
            switch (_id)
            {
                case LevelID.MAIN:
                    break;
                case LevelID.ENTRY:
                    break;
                case LevelID.LEVEL_MANNUAL_00:
                case LevelID.LEVEL_MANNUAL_01:
                case LevelID.LEVEL_MANNUAL_02:
                case LevelID.LEVEL_MANNUAL_03:
                case LevelID.LEVEL_MANNUAL_04:
                case LevelID.LEVEL_MANNUAL_05:
                case LevelID.LEVEL_MANNUAL_06:
                case LevelID.LEVEL_MANNUAL_07:
                case LevelID.LEVEL_MANNUAL_08:
                case LevelID.LEVEL_MANNUAL_09:
                case LevelID.LEVEL_MANNUAL_10:
                case LevelID.LEVEL_MANNUAL_11:
                case LevelID.LEVEL_MANNUAL_12:
                    GameManager.Instance.AudioSource?.Stop();
                    GameManager.Instance.AudioSource = SoundManager.Instance.PlayMusic(SoundID.LEVEL_THEME, 0.5f);
                    break;
                case LevelID.SHOP:
                case LevelID.HOME:
                    GameManager.Instance.AudioSource?.Stop();
                    GameManager.Instance.AudioSource = SoundManager.Instance.PlayMusic(SoundID.HOME_BG_MUSIC);
                    break;
                case LevelID.LEVEL_SELECTION:
                    GameManager.Instance.AudioSource?.Stop();
                    GameManager.Instance.AudioSource = SoundManager.Instance.PlayMusic(SoundID.LEVEL_SELECTION_BG_MUSIC, 0.65f);
                    break;
            }
        }
        public void TurnOnSeeBehind()
        {
            if (CustomizationManager.Instance == null)
                return;
            if (CustomizationManager.Instance.Display == null)
                return;
            CustomizationManager.Instance.Display.gameObject.layer = this.seebehindLayer;
        }
        public void TurnOffSeeBehind()
        {
            if (CustomizationManager.Instance == null)
                return;
            if (CustomizationManager.Instance.Display == null)
                return;
            CustomizationManager.Instance.Display.gameObject.layer = this.defaultLayer;
        }
    }
}
