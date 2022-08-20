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
        [SerializeField] LevelConfig m_LevelConfig;
        [SerializeField] LevelID m_CurrentLevelID;
        public LevelConfig LevelConfig => m_LevelConfig;
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

                // * Call Event to load Level
                GameEvents.LOAD_LEVEL?.Invoke(value);

                CurrentLevelData = m_LevelConfig.GetLevelDataByID(value);
            }
        }
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
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

        IEnumerator Cor_SetupLevelData(LevelData _data)
        {
            yield return new WaitUntil(() => UIManager.Instance != null);
            GameEvents.SETUP_LEVEL?.Invoke(_data);
        }
    }
}
