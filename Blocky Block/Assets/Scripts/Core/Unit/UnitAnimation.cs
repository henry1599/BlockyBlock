using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using NaughtyAttributes;

namespace BlockyBlock.Core
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        static readonly int TurnKey = Animator.StringToHash("Turn");
        static readonly int GrabKey = Animator.StringToHash("Grab");
        static readonly int ThrowKey = Animator.StringToHash("Throw");
        static readonly int RunKey = Animator.StringToHash("Run");
        static readonly int PushKey = Animator.StringToHash("Push");
        static readonly int IdleKey = Animator.StringToHash("Idle");
        static readonly int m_UpperLayerIdx = 1;
        static readonly int m_AboveLayerIdx = 0; 
        public void TriggerAnimTurn()
        {
            m_Animator.CrossFade(TurnKey, 0, m_AboveLayerIdx);
        }
        public void TriggerAnimUpperLayer(int _weight)
        {
            m_Animator.SetLayerWeight(m_UpperLayerIdx, _weight);
        }
        public void TriggerAnimPickup()
        {
            m_Animator.CrossFade(GrabKey, 0, m_UpperLayerIdx);
        }
        public void TriggerAnimPutdown()
        {
            m_Animator.CrossFade(ThrowKey, 0, m_UpperLayerIdx);
        }
        public void TriggerAnimRunning()
        {
            m_Animator.CrossFade(RunKey, 0, m_AboveLayerIdx);
        }
        public void TriggerAnimPush()
        {
            m_Animator.CrossFade(PushKey, 0, m_AboveLayerIdx);
        }
        public void Reset()
        {
            m_Animator.CrossFade(IdleKey, 0, m_AboveLayerIdx);
        }
    }
}
