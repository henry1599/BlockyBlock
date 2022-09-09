using System.Collections;
using System.Collections.Generic;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine;

namespace BlockyBlock.Tools
{
    public class ToolButton : MonoBehaviour
    {
        [SerializeField] CursorType m_Type;
        [SerializeField] ToolButtonAnimation m_ToolAnim;
        void Start()
        {
            ToolEvents.ON_CURSOR_CHANGED += HandleCursorChanged;
            HandToolManager.Instance.OnToolSelected(0);
        }
        void OnDestroy()
        {
            ToolEvents.ON_CURSOR_CHANGED -= HandleCursorChanged;
        }
        void HandleCursorChanged(CursorType _type)
        {
            m_ToolAnim?.ToggleAnimStatus(_type == m_Type);
        }
    }
}
