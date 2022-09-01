using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Managers;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.EventSystems;
using Utility.SLayout;

namespace BlockyBlock.UI
{
    public class UIBlockJump : UIBlock
    {
        public UIBlockSkip m_UIBlockJumpTo {get; set;}
        public ArrowJump m_ArrowJump;
        private bool m_IsPlaced = false;
        public override void Setup(UIBlock _parentBlock = null)
        {
            base.Setup(_parentBlock);
            if (!m_IsPlaced)
            {
                GameObject ideContent = GameObject.FindGameObjectWithTag(GameConstants.IDE_CONTENT_TAG);

                m_IsPlaced = true;

                m_UIBlockJumpTo = Instantiate(UIManager.Instance.m_BlockDatas[BlockType.SKIP].gameObject).GetComponent<UIBlockSkip>();
                m_UIBlockJumpTo.transform.SetParent(ideContent.transform);

                m_UIBlockJumpTo.Setup(this);
            }
            EnableArrow();
            SetupArrow();
            // * Spawn end jump block
        }
        private void Update() 
        {
            if (m_IsDragging)
            {
                DisableArrow();
                return;
            }
            if (m_UIBlockJumpTo != null)
            {
                if (m_UIBlockJumpTo.IsDragging)
                {
                    DisableArrow();
                    return;
                }
            }
            SetupArrow();
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            DisableArrow();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            EnableArrow();
            SetupArrow();
            base.OnEndDrag(eventData);
        }
        public void EnableArrow()
        {
            m_ArrowJump.EnableSelf();
        }
        public void SetupArrow()
        {
            StartCoroutine(Cor_SetupArrow());
        }
        IEnumerator Cor_SetupArrow()
        {
            yield return new WaitUntil(() => m_UIBlockJumpTo != null);
            m_ArrowJump.SetupSelf(m_UIBlockJumpTo);
        }
        public void DisableArrow()
        {
            m_ArrowJump.DisableSelf();
        }
        void OnDestroy() 
        {
            if (m_UIBlockJumpTo == null)
            {
                return;
            }
            m_UIBlockJumpTo.DestroySelf(m_UIBlockJumpTo.transform);
        }
    }
}

