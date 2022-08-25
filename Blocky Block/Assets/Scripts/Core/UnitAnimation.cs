using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.Core
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        private static readonly int MovingKeyAnimation = Animator.StringToHash("Moving");
        private static readonly int BlendKeyAnimation = Animator.StringToHash("Blend");
        void Flip(bool _isLeft)
        {
            transform.localScale = new Vector3(
                _isLeft ? 1 : -1,
                1,
                1
            );
        }
        public void TriggerMovingBool(bool _status)
        {
            m_Animator.SetBool(MovingKeyAnimation, _status);
        }
        public void TriggerBlendValue(float _value)
        {
            m_Animator.SetFloat(BlendKeyAnimation, _value);
        }
        public void SetDirection(UnitDirection _direction)
        {
            TriggerMovingBool(false);
            float blendValue = 0;
            switch (_direction)
            {
                case UnitDirection.UP:
                    blendValue = UnitConstants.BLEND_UP_VALUE;
                    break;
                case UnitDirection.LEFT:
                    blendValue = UnitConstants.BLEND_SIDE_VALUE;
                    break;
                case UnitDirection.RIGHT:
                    blendValue = UnitConstants.BLEND_SIDE_VALUE;
                    break;
                case UnitDirection.DOWN:
                    blendValue = UnitConstants.BLEND_DOWN_VALUE;
                    break;
            }
            Flip(_direction == UnitDirection.LEFT);
            TriggerBlendValue(blendValue);
        }
    }
}
