using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.Tools
{
    public class ZoomButton : MonoBehaviour
    {
        public ZoomType m_Type;
        [SerializeField] ToolButtonAnimation m_ToolAnim;
        // Start is called before the first frame update
        void Start()
        {
            ToolEvents.ON_ZOOM_BUTTON_CLICKED += HandleZoomButtonClick;
        }
        void OnDestroy()
        {
            ToolEvents.ON_ZOOM_BUTTON_CLICKED -= HandleZoomButtonClick;
        }
        void HandleZoomButtonClick(ZoomType _type)
        {
            if (m_Type != _type)
            {
                return;
            }
            m_ToolAnim?.PlayClickAnim();
        }
    }
}
