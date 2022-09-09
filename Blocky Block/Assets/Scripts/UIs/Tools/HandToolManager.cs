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
        public void OnToolSelected(int _idx)
        {
            ToolEvents.ON_CURSOR_CHANGED?.Invoke((CursorType)_idx);
        }
        public void OnZoomButtonSelected(int _idx)
        {
            ToolEvents.ON_ZOOM_BUTTON_CLICKED?.Invoke((ZoomType)_idx);
        }
    }
}
