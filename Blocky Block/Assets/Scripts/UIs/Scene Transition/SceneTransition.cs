using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioPlayer;
using BlockyBlock.Managers;

namespace BlockyBlock.UI
{
    public class SceneTransition : MonoBehaviour
    {
        public static readonly int InKeyAnimation = Animator.StringToHash("in");
        public static readonly int OutKeyAnimation = Animator.StringToHash("out");
        public static SceneTransition Instance {get; private set;}
        private Animator m_Animator;
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            m_Animator = GetComponent<Animator>();
        }
        public void TransitionIn()
        {
            SoundManager.Instance.PlaySound(SoundID.TRANSITION_IN);
            m_Animator.CrossFade(InKeyAnimation, 0, 0);
        }
        public void TransitionOut()
        {
            SoundManager.Instance.PlaySound(SoundID.TRANSITION_OUT);
            m_Animator.CrossFade(OutKeyAnimation, 0, 0);
        }
    }
}
