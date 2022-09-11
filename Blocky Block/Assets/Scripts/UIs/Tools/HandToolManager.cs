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
        public CursorType CurrentCursor = CursorType.SELECTION;
        void Awake()
        {
            Instance = this;
        }
        public void OnToolSelected(int _idx)
        {
            CurrentCursor = (CursorType)_idx;
            ToolEvents.ON_CURSOR_CHANGED?.Invoke((CursorType)_idx);
        }
        public void OnZoomButtonSelected(int _idx)
        {
            ToolEvents.ON_ZOOM_BUTTON_CLICKED?.Invoke((ZoomType)_idx);
        }
        public void OnResetButtonSelected()
        {
            ToolEvents.ON_RESET_BUTTON_CLICKED?.Invoke();
        }
    }
}
