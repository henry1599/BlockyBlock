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
        public RuntimeAnimatorController HomeAnim;
        public RuntimeAnimatorController LevelSelectionAnim;
        public RuntimeAnimatorController LevelAnim;
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            // StartCoroutine(Cor_UpdateSceneID());
        }
        // Start is called before the first frame update
        void Start()
        {
            TransitionOut();
            GameEvents.ON_WIN += HandleWin;
            GameEvents.ON_LOSE += HandleLose;
            GameEvents.LOAD_LEVEL += HandleLoadLevel;
        }
        void OnDestroy()
        {
            GameEvents.ON_WIN -= HandleWin;
            GameEvents.ON_LOSE -= HandleLose;
            GameEvents.LOAD_LEVEL -= HandleLoadLevel;
        }
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
            yield return new WaitUntil(() => LevelManager.Instance != null && 
                                             ConfigManager.Instance != null);
            LevelManager.Instance.CurrentLevelID = _id;
        }
    }
}
