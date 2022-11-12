using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using AudioPlayer;

namespace BlockyBlock.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance {get; private set;}
        LevelID m_CurrentLevelID;
        public LevelConfig LevelConfig => ConfigManager.Instance.LevelConfig;
        LevelData m_CurrentLevelData;
        private bool isSoundMapLoaded = false;
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
            Instance = this;
            StartCoroutine(Cor_LoadSoundMap());
            UnitEvents.ON_COLLECT_STUFF += HandleCollectStuff;
            UnitEvents.ON_RESET += HandleReset;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_CurrentLevelID = LevelID.HOME;
            StartCoroutine(Cor_PlayBGMusic());
        }
        IEnumerator Cor_LoadSoundMap()
        {
            yield return new WaitUntil(() => SoundManager.Instance != null);
            SoundManager.Instance.LoadSoundMap(SoundType.HOME); 
            SoundManager.Instance.LoadSoundMap(SoundType.LEVEL);
            isSoundMapLoaded = true; 
        }
        IEnumerator Cor_PlayBGMusic()
        {
            yield return new WaitUntil(() => isSoundMapLoaded == true);
            switch (m_CurrentLevelID)
            {
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
                    SoundManager.Instance.PlayMusic(SoundID.LEVEL_THEME);
                    break;
                case LevelID.HOME:
                    SoundManager.Instance.PlayMusic(SoundID.HOME_BG_MUSIC);
                    break;
                case LevelID.LEVEL_SELECTION:
                    SoundManager.Instance.PlayMusic(SoundID.LEVEL_SELECTION_BG_MUSIC);
                    break;
            }
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
                                             LevelManager.Instance != null);
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
    }
}
