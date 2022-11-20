using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.SceneManagement;
using BlockyBlock.UI;
using AudioPlayer;

namespace BlockyBlock.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}
        public RuntimeAnimatorController HomeAnim;
        public RuntimeAnimatorController LevelSelectionAnim;
        public RuntimeAnimatorController LevelAnim;
        public AudioSource AudioSource {get; set;}
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

            SoundManager.ON_FINISH_LOADING_SOUNDMAP += HandleFinishLoadingSoundmap;
        }
        void HandleFinishLoadingSoundmap()
        {
            AudioSource = SoundManager.Instance.PlayMusic(SoundID.HOME_BG_MUSIC);
        }
        void OnDestroy()
        {
            GameEvents.ON_WIN -= HandleWin;
            GameEvents.ON_LOSE -= HandleLose;
            GameEvents.LOAD_LEVEL -= HandleLoadLevel;

            SoundManager.ON_FINISH_LOADING_SOUNDMAP -= HandleFinishLoadingSoundmap;
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
            yield return new WaitUntil(() => ConfigManager.Instance != null);
            LevelManager.Instance.CurrentLevelID = _id;
            LevelManager.Instance.ResetChecker();
            PlayBGMusic(_id);
        }
        void PlayBGMusic(LevelID _id)
        {
            switch (_id)
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
                    GameManager.Instance.AudioSource?.Stop();
                    GameManager.Instance.AudioSource = SoundManager.Instance.PlayMusic(SoundID.LEVEL_THEME, 0.5f);
                    break;
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
    }
}
