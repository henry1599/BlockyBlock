using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using AudioPlayer;
using BlockyBlock.Tracking;

namespace BlockyBlock.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance {get; private set;}
        LevelID m_CurrentLevelID;
        public LevelConfig LevelConfig => ConfigManager.Instance.LevelConfig;
        LevelData m_CurrentLevelData;
        private bool isSoundMapLoaded = false;
        private FromState[] possibleEntries = new FromState[] {
            FromState.Level_selection,
            FromState.Previous_level,
            FromState.Replay_current_level
        };
        public LevelFinishedRecordData LevelFinished => TrackingManager.Instance.Helper.LevelFinished;
        public LevelTriggerRecordData LevelTrigger => TrackingManager.Instance.Helper.LevelTrigger;
        public LevelData CurrentLevelData
        {
            get => m_CurrentLevelData;
            set 
            {
                m_CurrentLevelData = value;

                if (value.LevelType == LevelType.HOME)
                {
                    return;
                }
                // * Call Event to spawn Preview Block on UI side
                StartCoroutine(Cor_SetupLevelData(value));
            }
        }
        public LevelID CurrentLevelID 
        {
            get => m_CurrentLevelID;
            set 
            {
                m_CurrentLevelID = value;

                CurrentLevelData = ConfigManager.Instance.LevelConfig.GetLevelDataByID(value);
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
            StartCoroutine(Cor_LoadSoundMap());
            UnitEvents.ON_COLLECT_STUFF += HandleCollectStuff;
            UnitEvents.ON_RESET += HandleReset;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_CurrentLevelID = LevelID.HOME;
        }
        IEnumerator Cor_LoadSoundMap()
        {
            yield return new WaitUntil(() => SoundManager.Instance != null);
            SoundManager.Instance.LoadSoundMap(SoundType.HOME); 
            SoundManager.Instance.LoadSoundMap(SoundType.LEVEL);
            isSoundMapLoaded = true; 
        }
        void OnDestroy()
        {
            UnitEvents.ON_COLLECT_STUFF -= HandleCollectStuff;
            UnitEvents.ON_RESET -= HandleReset;
        }
        void HandleReset()
        {
            CurrentCollectedStuff = 0;
            IsCollectTheChest = false;
            IsReachToPosition = false;
        }
        void HandleCollectStuff()
        {
            CurrentCollectedStuff++;
            LevelChecker();
        }

        IEnumerator Cor_SetupLevelData(LevelData _data)
        {
            yield return new WaitUntil(() => UIManager.Instance != null &&
                                             UnitManager.Instance != null &&
                                             LevelReader.Instance != null && 
                                             ConfigManager.Instance != null &&
                                             GroundManager.Instance != null);
            GameEvents.SETUP_LEVEL?.Invoke(_data);
        }
        void LevelChecker()
        {
            switch (CurrentLevelData.WinCondition)
            {
                case WinType.COLLECT_ALL_STUFF: 
                    CheckCollectAllStuff();
                    break;
                case WinType.COLLECT_THE_CHEST: 
                    CheckCollectTheChest();
                    break;
                case WinType.REACH_TO_POSITION: 
                    CheckReachToPosition();
                    break;
            }
        }
        public void ResetChecker()
        {
            CurrentCollectedStuff = 0;
            IsCollectTheChest = false;
            IsReachToPosition = false;
        }

        // * Collect all stuff
        public int CurrentCollectedStuff { get => m_CurrentCollectedStuff; set => m_CurrentCollectedStuff = value; } int m_CurrentCollectedStuff = 0;
        void CheckCollectAllStuff()
        {
            if (CurrentCollectedStuff >= CurrentLevelData.StuffToCollect)
            {
                GameEvents.ON_WIN?.Invoke();
            }
        }

        // * Collect the chest
        public bool IsCollectTheChest {get => m_IsCollectTheChest; set => m_IsCollectTheChest = value;} bool m_IsCollectTheChest = false;
        void CheckCollectTheChest()
        {
            if (IsCollectTheChest)
            {
                GameEvents.ON_WIN?.Invoke();
            }
        }
        // * Reach to Position
        public bool IsReachToPosition {get => m_IsReachToPosition; set => m_IsReachToPosition = value;} bool m_IsReachToPosition = false;
        void CheckReachToPosition()
        {
            if (IsReachToPosition)
            {
                GameEvents.ON_WIN?.Invoke();
            }
        }

        #region TRACKING
        // * Level trigger
        float levelTimeSpentStart = 0;
        public void StartTimer()
        {
            this.levelTimeSpentStart = Time.time;
        }
        public void SetLevelIDLevelTrigger()
        {
            LevelTrigger.levelId = CurrentLevelID.ToString();
        }
        public void SetChapterIDLevelTrigger()
        {
            LevelTrigger.chapterId = CurrentLevelData.ChapterID.ToString();
        }
        public void SetEntryLevelTrigger()
        {
            LevelTrigger.entry = this.possibleEntries[0].ToString();
            foreach (var state in this.possibleEntries)
            {
                if (GameManager.Instance.IsFrom(state))
                {
                    LevelTrigger.entry = state.ToString();
                    return;
                }
            }
        }
        public void SetLevelIDLevelFinished()
        {
            LevelFinished.levelId = CurrentLevelID.ToString();
        }
        public void SetChapterIDLevelFinished()
        {
            LevelFinished.chapterId = CurrentLevelData.ChapterID.ToString();
        }
        public void SetEntryLevelFinished()
        {
            LevelFinished.entry = this.possibleEntries[0].ToString();
            foreach (var state in this.possibleEntries)
            {
                if (GameManager.Instance.IsFrom(state))
                {
                    LevelFinished.entry = state.ToString();
                    return;
                }
            }
        }
        public void SetDebugButtonCount()
        {
            LevelFinished.debugButtonCount++;
        }
        public void SetEndCauseLevelFinished(EndCause endCause)
        {
            LevelFinished.endCause = endCause.ToString();
        }
        public void SetPickupBlockCount()
        {
            LevelFinished.pickUpBlockCount++;
        }
        public void SetPickupBlockCountUse()
        {
            LevelFinished.pickUpBlockCountUse++;
        }
        public void SetPlayButtonCount()
        {
            LevelFinished.playButtonCount++;
        }
        public void SetPushBlockCount()
        {
            LevelFinished.pushBlockCount++;
        }
        public void SetPushBlockCountUse()
        {
            LevelFinished.pushBlockCountUse++;
        }
        public void SetPutDownBlockCount()
        {
            LevelFinished.putDownBlockCount++;
        }
        public void SetPutDownBlockCountUse()
        {
            LevelFinished.putDownBlockCountUse++;
        }
        public void SetStepForwardBlockCount()
        {
            LevelFinished.stepForwardBlockCount++;
        }
        public void SetStepForwardBlockCountUse()
        {
            LevelFinished.stepForwardBlockCountUse++;
        }
        public void SetStopButtonCount()
        {
            LevelFinished.stopButtonCount++;
        }
        public void SetTimeSpent()
        {
            float endRecordTime = Time.time;
            float totalTime = endRecordTime - this.levelTimeSpentStart;
            LevelFinished.timeSpent = (int)totalTime;
        }
        public void SetTurnLeftBlockCount()
        {
            LevelFinished.turnLeftBlockCount++;
        }
        public void SetTurnLeftBlockCountUse()
        {
            LevelFinished.turnLeftBlockCountUse++;
        }
        public void SetTurnRightBlockCount()
        {
            LevelFinished.turnRightBlockCount++;
        }
        public void SetTurnRightBlockCountUse()
        {
            LevelFinished.turnRightBlockCountUse++;
        }
        public void SetJumpBlockCount()
        {
            LevelFinished.jumpBlockCount++;
        }
        public void SetJumpBlockCountUse()
        {
            LevelFinished.jumpBlockCountUse++;
        }
        public void SetJumpIfBlockCount()
        {
            LevelFinished.jumpIfBlockCount++;
        }
        public void SetJumpIfBlockCountUse()
        {
            LevelFinished.jumpIfBlockCountUse++;
        }
        public void SetIsProgress(bool isInProgress)
        {
            LevelFinished.isProgress = isInProgress;
        }
        #endregion
    }
}
