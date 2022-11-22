using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.UI;

namespace BlockyBlock.UI
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField] Animator m_Animator;
        [SerializeField] ControlButton m_Type;
        [SerializeField] Button m_ThisButton;
        public static readonly int StatusKeyAnimation = Animator.StringToHash("status");
        public static readonly int DisableKeyAnimation = Animator.StringToHash("disable");
        void Start()
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE += HandleControlButtonClick;
            GameEvents.ON_CONTROL_BUTTON_TOGGLE_ALL += HandleControlButtonClickAll;
            GameEvents.ON_WIN += HandleFreeze;
            GameEvents.ON_LOSE += HandleFreeze;
            
            ErrorEvents.ON_ERROR_HANDLING += HandleError;
        }
        void OnDestroy()
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE -= HandleControlButtonClick;
            GameEvents.ON_CONTROL_BUTTON_TOGGLE_ALL -= HandleControlButtonClickAll;
            GameEvents.ON_WIN -= HandleFreeze;
            GameEvents.ON_LOSE -= HandleFreeze;

            ErrorEvents.ON_ERROR_HANDLING -= HandleError;
        }
        void HandleFreeze()
        {
            SetStatus(true);
        }
        void HandleError()
        {
            ActiveSelf(false);
        }
        void HandleControlButtonClickAll(bool _status)
        {
            SetStatus(_status);
            m_ThisButton.interactable = !_status;
        }
        void HandleControlButtonClick(ControlButton _type, bool _status)
        {
            if (_type != m_Type)
            {
                return;
            }
            SetStatus(_status);
            m_ThisButton.interactable = !_status;
        }
        public void SetStatus(bool _status)
        {
            m_Animator.SetBool(StatusKeyAnimation, _status);
        }
        public void ActiveSelf(bool _status)
        {
            m_Animator.SetBool(DisableKeyAnimation, _status);
        }
    }
}
