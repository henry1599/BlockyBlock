using System.Collections;
using System.Collections.Generic;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine;

namespace BlockyBlock.Tools
{
    public class ResetButton : MonoBehaviour
    {
        [SerializeField] ToolButtonAnimation m_ToolAnim;
        // Start is called before the first frame update
        void Start()
        {
            ToolEvents.ON_RESET_BUTTON_CLICKED += HandleResetButtonClick;
        }
        void OnDestroy()
        {
            ToolEvents.ON_RESET_BUTTON_CLICKED -= HandleResetButtonClick;
        }
        void HandleResetButtonClick()
        {
            m_ToolAnim?.PlayClickAnim();
        }
    }
}
