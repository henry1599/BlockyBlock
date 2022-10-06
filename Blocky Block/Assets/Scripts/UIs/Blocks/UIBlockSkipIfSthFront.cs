using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Managers;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.EventSystems;
using Helpers;

namespace BlockyBlock.UI 
{
    public class UIBlockSkipIfSthFront : UIBlock
    {
        public UIBlockJumpIfSthFront m_UIBlockJumpFrom {get; set;} = null;
        public override bool IsDragging => m_IsDragging;
        public RectTransform TopPanel;
        public Image Arrow;
        protected override void Start() 
        {
            base.Start();
            Mode = Enums.BlockMode.IDE;
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            // * Disabled Arrow
            m_UIBlockJumpFrom.DisableArrow();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            // * Update arrow
            m_UIBlockJumpFrom.EnableArrow();
            m_UIBlockJumpFrom.SetupArrow();
        }
        void OnDestroy()
        {
            if (m_UIBlockJumpFrom == null)
            {
                return;
            }
            m_UIBlockJumpFrom.DestroySelf(m_UIBlockJumpFrom.gameObject);
        }
        public override void Setup(UIBlock _parentBlock = null)
        {
            // * Spawn end jump block
            base.Setup(_parentBlock);
            if (m_UIBlockJumpFrom == null)
            {
                m_UIBlockJumpFrom = (UIBlockJumpIfSthFront)_parentBlock;
            }
            m_IsDragging = false;
            m_DragOffset = Vector3.zero;
            m_CanvasGroup.blocksRaycasts = true;
            ToggleChildrenRaycastTarget(true);
        }
        public override void ClickSelf()
        {
            base.ClickSelf();
            m_UIBlockJumpFrom.m_Connection.line.color = m_UIBlockJumpFrom.ClickedColor;
            Arrow.color = m_UIBlockJumpFrom.ClickedColor;
        }
        public override void UnclickSelf()
        {
            base.UnclickSelf();
            m_UIBlockJumpFrom.m_Connection.line.color = m_UIBlockJumpFrom.UnclickedColor;
            Arrow.color = m_UIBlockJumpFrom.UnclickedColor;
        }
    }
}
