using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlockyBlock.UI 
{
    public class UIBlockSkip : UIBlock
    {
        public UIBlockJump m_UIBlockJumpFrom {get; set;} = null;
        public bool IsDragging => m_IsDragging;
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
            m_UIBlockJumpFrom.DestroySelf(m_UIBlockJumpFrom.transform);
        }
        public override void Setup(UIBlock _parentBlock = null)
        {
            // * Spawn end jump block
            if (m_UIBlockJumpFrom == null)
            {
                m_UIBlockJumpFrom = (UIBlockJump)_parentBlock;
            }
            m_IsDragging = false;
            m_DragOffset = Vector3.zero;
            m_CanvasGroup.blocksRaycasts = true;
            ToggleChildrenRaycastTarget(true);
        }
    }
}
