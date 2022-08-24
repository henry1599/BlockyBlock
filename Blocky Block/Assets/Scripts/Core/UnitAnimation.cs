using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Core
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        private static readonly int MovingKeyAnimation = Animator.StringToHash("Moving");
        private static readonly int BlendKeyAnimation = Animator.StringToHash("Blend");
        public void TriggerMovingBool(bool _status)
        {
            m_Animator.SetBool(MovingKeyAnimation, _status);
        }
        public void TriggerBlendValue(float _value)
        {
            m_Animator.SetFloat(BlendKeyAnimation, _value);
        }
    }
}
