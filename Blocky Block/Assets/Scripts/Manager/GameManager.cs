using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.SceneManagement;

namespace BlockyBlock.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}
        void Awake()
        {
            Instance = this;
            StartCoroutine(Cor_UpdateSceneID());
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        IEnumerator Cor_UpdateSceneID()
        {
            yield return new WaitUntil(() => LevelManager.Instance != null && 
                                             ConfigManager.Instance != null);
            string sceneName = SceneManager.GetActiveScene().name;
            LevelManager.Instance.CurrentLevelID = ConfigManager.Instance.LevelConfig.GetLevelIDBySceneName(sceneName);
        }
    }
}
