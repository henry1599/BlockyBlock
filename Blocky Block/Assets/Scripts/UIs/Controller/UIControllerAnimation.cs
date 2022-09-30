using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;

namespace BlockyBlock.UI 
{
    public class UIControllerAnimation : MonoBehaviour
    {
        private static readonly int StatusKeyAnimation = Animator.StringToHash("status");
        [SerializeField] Animator m_Anim;
        // Start is called before the first frame update
        void Start()
        {
            GameEvents.ON_TOGGLE_CONTROLLER_PANEL += HandleToggleController;
            
            GameEvents.ON_WIN += HandleStopRunning;
            GameEvents.ON_LOSE += HandleStopRunning;
        }
        void OnDestroy()
        {
            GameEvents.ON_TOGGLE_CONTROLLER_PANEL -= HandleToggleController;
            
            GameEvents.ON_WIN -= HandleStopRunning;
            GameEvents.ON_LOSE -= HandleStopRunning;
        }
        void HandleStopRunning()
        {
            HandleToggleController(false);
        }
        void HandleToggleController(bool _status)
        {
            m_Anim?.SetBool(StatusKeyAnimation, _status);
        }
    }
}
