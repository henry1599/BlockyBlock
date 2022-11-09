using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance {get; private set;}
        LevelID m_CurrentLevelID;
        public LevelConfig LevelConfig => ConfigManager.Instance.LevelConfig;
        LevelData m_CurrentLevelData;
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
            UnitEvents.ON_COLLECT_STUFF += HandleCollectStuff;
            UnitEvents.ON_RESET += HandleReset;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_CurrentLevelID = LevelID.HOME;
        }

        // Update is called once per frame
        void Update()
        {
            
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
