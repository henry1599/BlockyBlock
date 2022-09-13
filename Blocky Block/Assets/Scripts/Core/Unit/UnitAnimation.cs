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
        public void TriggerAnimSwimIdle()
        {
            m_Animator.CrossFade("Swim_idle", 0, 0);
        }
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
        public void TriggerAnimSwimming()
        {
            m_Animator.CrossFade("Swim_F", 0, 0);
        }
        public void TriggerAnimWaterToGround()
        {
            m_Animator.CrossFade("InPlace", 0, 0);
        }
        [Button]
        public void TriggerAnimGroundToWater()
        {
            m_Animator.CrossFade("Swim_Enter", 0, 0);
        }
        public void Reset(bool _underwater)
        {
            if (_underwater)
            {
                m_Animator.CrossFade("Swim_idle", 0, 0);
            }
            else
            {
                m_Animator.CrossFade("Base", 0, 0);
            }
        }
    }
}
