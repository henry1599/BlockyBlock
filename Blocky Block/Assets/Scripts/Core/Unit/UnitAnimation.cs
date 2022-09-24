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
        public void TriggerAnimTurn()
        {
            m_Animator.CrossFade("Turn", 0, 0);
        }
        public void TriggerAnimRunning()
        {
            m_Animator.CrossFade("Run", 0, 0);
        }
        public void Reset(bool _underwater)
        {
            m_Animator.CrossFade("Idle", 0, 0);
        }
    }
}
