using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        [SerializeField] GameObject container;
        [SerializeField] GameObject character;
        [SerializeField] Transform summaryBoard;
        [SerializeField] RuntimeAnimatorController characterAnim;
        [SerializeField] Animator transitionAnim;
        [SerializeField] GameObject normalCamera;
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
        public void Show()
        {
            StartCoroutine(Cor_Show());
            ProfileManager.Instance.SaveProfile();
        }
        IEnumerator Cor_Show()
        {
            this.container.gameObject.SetActive(true);
            this.transitionAnim.CrossFade(InTransitionKey, 0, 0);
            // AudioPlayer.SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_CLOUD_IN);
            // curToy = defaultToy;
            this.normalCamera.SetActive(true);

            yield return Helpers.Helper.GetWait(1f);
            CreateCharacter();
            StartCoroutine(Cor_ShowSummary());
            yield return new WaitForSeconds(0.5f);
            // SoundManager.Instance.PlaySound(SoundID.SFX_MG_END_CONFETTI);
        }
        void CreateCharacter()
        {
            this.character.SetActive(true);
            this.character.GetComponentInChildren<Animator>().runtimeAnimatorController = this.characterAnim;
            this.character.GetComponentInChildren<Animator>().CrossFade(DanceTransitionKeys[Random.Range(0, DanceTransitionKeys.Length)], 0, 0);
        }
        IEnumerator Cor_ShowSummary()
        {
            yield return Helpers.Helper.GetWait(0.5f);
            this.summaryBoard.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }
}
