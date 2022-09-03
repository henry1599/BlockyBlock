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
        public void TriggerAnimTurnLeft()
        {
            m_Animator.CrossFade("Trot_L", 0, 0);
        }
        public void TriggerAnimTurnRight()
        {
            m_Animator.CrossFade("Trot_R", 0, 0);
        }
        public void TriggerAnimRunning()
        {
            m_Animator.CrossFade("Trot_F", 0, 0);
        }
        public void Reset()
        {
            m_Animator.CrossFade("Base", 0, 0);
        }
    }
}
