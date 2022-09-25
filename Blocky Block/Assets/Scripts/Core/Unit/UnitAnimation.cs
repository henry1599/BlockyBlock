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
        static readonly int m_UpperLayerIdx = 1;
        static readonly int m_AboveLayerIdx = 0; 
        public void TriggerAnimTurn()
        {
            m_Animator.CrossFade("Turn", 0, m_AboveLayerIdx);
        }
        public void TriggerAnimUpperLayer(int _weight)
        {
            m_Animator.SetLayerWeight(m_UpperLayerIdx, _weight);
        }
        public void TriggerAnimPickup()
        {
            m_Animator.CrossFade("Grab", 0, m_UpperLayerIdx);
        }
        public void TriggerAnimPutdown()
        {
            m_Animator.CrossFade("Throw", 0, m_UpperLayerIdx);
        }
        public void TriggerAnimRunning()
        {
            m_Animator.CrossFade("Run", 0, m_AboveLayerIdx);
        }
        public void Reset()
        {
            m_Animator.CrossFade("Idle", 0, m_AboveLayerIdx);
        }
    }
}
