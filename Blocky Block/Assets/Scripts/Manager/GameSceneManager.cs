using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance {get; private set;}
        void Awake()
        {
            Instance = this;
            GameEvents.LOAD_LEVEL += HandleLoadLevel;
        }
        void OnDestroy()
        {
            GameEvents.LOAD_LEVEL -= HandleLoadLevel;
        }
        void HandleLoadLevel(LevelID _id)
        {
            LoadSceneByID(_id);
        }
        void LoadSceneByID(LevelID _id)
        {
            string sceneName = ConfigManager.Instance.SceneConfig.GetSceneNameByID(_id);
            SceneManager.LoadScene(sceneName);
        }
    }
}
