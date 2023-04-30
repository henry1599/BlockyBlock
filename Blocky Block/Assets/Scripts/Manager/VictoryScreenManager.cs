using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;
using BlockyBlock.Configurations;
using BlockyBlock.UI;
using BlockyBlock.Utils;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using AudioPlayer;
using BlockyBlock.Tracking;

namespace BlockyBlock.Managers
{
    public class VictoryScreenManager : MonoBehaviour
    {
        public static VictoryScreenManager Instance {get; private set;}
        private static readonly int InTransitionKey = Animator.StringToHash("in");
        private static readonly int[] DanceTransitionKeys = new int[]
        {
            Animator.StringToHash("Dance01"),
            Animator.StringToHash("Dance02"),
            Animator.StringToHash("Dance03"),
            Animator.StringToHash("Dance04")
        };
        [SerializeField] Button backButton;
        [SerializeField] Button restartButton;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject container;
        [SerializeField] GameObject character;
        [SerializeField] UISummary summaryBoard;
        [SerializeField] RuntimeAnimatorController characterAnim;
        [SerializeField] Animator transitionAnim;
        [SerializeField] GameObject normalCamera;
        private LevelID currentId;
        public System.Action OnExit;
        public System.Action OnRestart;
        public System.Action OnNext;
        void Awake()
        {
            Instance = this;
        }
        void OnDestroy()
        {
            RemoveAllListeners(ref OnExit);
            RemoveAllListeners(ref OnRestart);
            RemoveAllListeners(ref OnNext);
            Instance = null;
        }
        private void RemoveAllListeners(ref System.Action action)
        {
            if (action == null) return;

            var listeners = action.GetInvocationList();
            foreach (var l in listeners)
            {
                action -= (System.Action)l;
            }
        }
        public void Show(bool winGame = true, bool usedBlockPassed = false, bool stepPassed = false)
        {
            StartCoroutine(Cor_Show(winGame, usedBlockPassed, stepPassed));
            ProfileManager.Instance.SaveProfile();

            string sceneName = SceneManager.GetActiveScene().name;
            this.currentId = ConfigManager.Instance.SceneConfig.GetLevelIDBySceneName(sceneName);
            ChapterID currentChapterId = (ChapterID)PlayerPrefs.GetInt(GameConstants.CHAPTER_CHOSEN_KEY, 0);
            switch (currentChapterId)
            {
                case ChapterID.CHAPTER_01:
                    nextButton.gameObject.SetActive(this.currentId < LevelID.LEVEL_MANNUAL_05);
                    break;
                case ChapterID.CHAPTER_02:
                    // nextButton.gameObject.SetActive(this.currentId < LevelID.LEVEL_MANNUAL_05);
                    break;
                case ChapterID.CHAPTER_03:
                    // nextButton.gameObject.SetActive(this.currentId < LevelID.LEVEL_MANNUAL_05);
                    break;
            }

            backButton.onClick.AddListener(() => OnBackButtonClick());
            restartButton.onClick.AddListener(() => OnRestartButtonClick());
            nextButton.onClick.AddListener(() => OnNextButtonClick());
        }
        IEnumerator Cor_Show(bool winGame = true, bool usedBlockPassed = false, bool stepPassed = false)
        {
            this.container.gameObject.SetActive(true);
            SoundManager.Instance.PlaySound(SoundID.TRANSITION_IN);
            this.transitionAnim.CrossFade(InTransitionKey, 0, 0);
            this.normalCamera.SetActive(true);

            yield return Helpers.Helper.GetWait(1f);
            SoundManager.Instance.PlaySound(SoundID.CONFETTI);
            // SoundManager.Instance.PlaySound(SoundID.WIN);
            CreateCharacter();
            StartCoroutine(Cor_ShowSummary(winGame, usedBlockPassed, stepPassed));
            yield return new WaitForSeconds(0.5f);
        }
        void CreateCharacter()
        {
            this.character.SetActive(true);
            this.character.GetComponentInChildren<Animator>().runtimeAnimatorController = this.characterAnim;
            this.character.GetComponentInChildren<Animator>().CrossFade(DanceTransitionKeys[Random.Range(0, DanceTransitionKeys.Length)], 0, 0);
            CustomizationManager.Instance.SetCustomization(this.character.GetComponentInChildren<CustomizationDisplay>());
        }
        IEnumerator Cor_ShowSummary(bool winGame = true, bool usedBlockPassed = false, bool stepPassed = false)
        {
            yield return Helpers.Helper.GetWait(0.5f);
            this.summaryBoard.Setup(winGame, usedBlockPassed, stepPassed);
            this.summaryBoard.transform
                .DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        public void OnBackButtonClick()
        {
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    PlayerPrefs.SetInt(GameConstants.LEVEL_TO_BACK_KEY, (int)this.currentId);
                    GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_SELECTION);
                }
            );
            LevelManager.Instance.SetEndCauseLevelFinished(EndCause.Level_selection_button);
            GameManager.Instance.UnsetFromState(FromState.Level_selection, FromState.Replay_current_level, FromState.Previous_level);
        }
        public void OnNextButtonClick()
        {
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    
                    GameEvents.LOAD_LEVEL?.Invoke((LevelID)((int)this.currentId + 1));
                }
            );
            LevelManager.Instance.SetEndCauseLevelFinished(EndCause.Next_button);
            GameManager.Instance.UnsetFromState(FromState.Level_selection, FromState.Replay_current_level);
        }
        public void OnRestartButtonClick()
        {
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    string sceneName = SceneManager.GetActiveScene().name;
                    LevelID currentId = ConfigManager.Instance.SceneConfig.GetLevelIDBySceneName(sceneName);
                    GameEvents.LOAD_LEVEL?.Invoke((LevelID)currentId);
                }
            );
            LevelManager.Instance.SetEndCauseLevelFinished(EndCause.Replay_button);
            GameManager.Instance.UnsetFromState(FromState.Level_selection, FromState.Previous_level);
            GameManager.Instance.SetFromState(FromState.Replay_current_level);
        }
    }
}
