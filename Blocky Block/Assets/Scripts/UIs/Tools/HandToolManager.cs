using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Tools
{
    public class HandToolManager : MonoBehaviour
    {
        public static HandToolManager Instance {get; private set;}
        void Awake()
        {
            Instance = this;
        }
        public void OnResetButtonSelected()
        {
            ToolEvents.ON_RESET_BUTTON_CLICKED?.Invoke();
        }
    }
}
