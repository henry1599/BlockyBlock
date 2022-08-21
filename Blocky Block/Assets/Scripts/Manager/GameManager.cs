using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}
        [Header("Current Scene ID On Start (Debug Only)")]
        [SerializeField] LevelID m_LevelID;
        [Header("Managers")]
        [Tooltip("Spawn this prefab if it is not in the scene as playing")]
        [SerializeField] LevelManager m_LevelManager;
        [Tooltip("Spawn this prefab if it is not in the scene as playing")]
        [SerializeField] GameSceneManager m_GameSceneManager;
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
            GameObject levelManager = GameObject.FindGameObjectWithTag("Level Manager");
            GameObject gameSceneManager = GameObject.FindGameObjectWithTag("Scene Manager");
            if (levelManager == null)
            {
                Instantiate(m_LevelManager.gameObject);
                StartCoroutine(Cor_UpdateSceneID());
            }
            if (gameSceneManager == null)
            {
                Instantiate(m_GameSceneManager.gameObject);
            }
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
            yield return new WaitUntil(() => LevelManager.Instance != null);
            LevelManager.Instance.CurrentLevelID = m_LevelID;
        }
    }
}
