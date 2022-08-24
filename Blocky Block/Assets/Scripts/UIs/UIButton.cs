using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.UI
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField] Animator m_Animator;
        [SerializeField] ControlButton m_Type;
        public static readonly int StatusKeyAnimation = Animator.StringToHash("status");
        void Start()
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE += HandleControlButtonClick;
        }
        void OnDestroy()
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE -= HandleControlButtonClick;
        }
        void HandleControlButtonClick(ControlButton _type, bool _status)
        {
            if (_type != m_Type)
            {
                return;
            }
            SetStatus(_status);
        }
        public void SetStatus(bool _status)
        {
            m_Animator.SetBool(StatusKeyAnimation, _status);
        }
    }
}
