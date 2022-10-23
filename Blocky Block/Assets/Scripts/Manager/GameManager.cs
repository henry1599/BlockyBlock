using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.SceneManagement;
using BlockyBlock.UI;

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
            TransitionOut();
            GameEvents.ON_WIN += HandleWin;
            GameEvents.ON_LOSE += HandleLose;
        }
        void OnDestroy()
        {
            GameEvents.ON_WIN -= HandleWin;
            GameEvents.ON_LOSE -= HandleLose;
        }
        void HandleWin()
        {
            Debug.Log("Win");
        }
        void HandleLose()
        {
            Debug.Log("Lose");
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
        IEnumerator Cor_UpdateSceneID()
        {
            yield return new WaitUntil(() => LevelManager.Instance != null && 
                                             ConfigManager.Instance != null);
            string sceneName = SceneManager.GetActiveScene().name;
            LevelManager.Instance.CurrentLevelID = ConfigManager.Instance.LevelConfig.GetLevelIDBySceneName(sceneName);
        }
    }
}
