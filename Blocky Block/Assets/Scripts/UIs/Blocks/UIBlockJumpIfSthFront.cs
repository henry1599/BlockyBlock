using System.Collections;
using UnityEngine;
using BlockyBlock.Managers;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using UnityEngine.EventSystems;
using Helpers;

namespace BlockyBlock.UI 
{
    public class UIBlockJumpIfSthFront : UIBlock
    {
        public UIBlockSkipIfSthFront m_UIBlockJumpTo {get; set;}
        public float MinWeightConnectionFrom;
        public float MaxWeightConnectionFrom;
        public float MinWeightConnectionTo;
        public float MaxWeightConnectionTo;
        public UIBlockOptionSthFront UIBlockOptionSthFront;
        // public ArrowJump m_ArrowJump;
        private bool m_IsPlaced = false;
        public Connection m_Connection;
        public Color ClickedColor, UnclickedColor;
        public RectTransform TopPanel;
        private float m_WeightConnectionFrom = -1;
        private float m_WeightConnectionTo = 0;
        public override void Setup(UIBlock _parentBlock = null)
        {
            base.Setup(_parentBlock);
            ((UIBlockOptionSthFront)UIBlockOption).Setup();
            
            if (m_WeightConnectionTo == 0)
            {
                m_WeightConnectionTo = Random.Range(MinWeightConnectionTo, MaxWeightConnectionTo);
            }
            if (m_WeightConnectionFrom == -1)
            {
                m_WeightConnectionFrom = Random.Range(MinWeightConnectionFrom, MaxWeightConnectionFrom);
            }

            if (!m_IsPlaced)
            {
                GameObject ideContent = GameObject.FindGameObjectWithTag(GameConstants.IDE_CONTENT_TAG);

                m_IsPlaced = true;

                m_UIBlockJumpTo = Instantiate(UIManager.Instance.m_BlockDatas[BlockType.SKIP_IF_STH_FRONT].gameObject, transform.position, Quaternion.identity, ideContent.transform).GetComponent<UIBlockSkipIfSthFront>();
                m_UIBlockJumpTo.transform.SetParent(ideContent.transform);

                m_UIBlockJumpTo.Setup(this);
            }
            EnableArrow();
            SetupArrow();
            // * Spawn end jump block
        }
        private void Update() 
        {
            base.Update();
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
        public override void ClickSelf()
        {
            base.ClickSelf();
            m_Connection.line.color = ClickedColor;
            m_UIBlockJumpTo.Arrow.color = ClickedColor;
        }
        public override void UnclickSelf()
        {
            base.UnclickSelf();
            m_Connection.line.color = UnclickedColor;
            m_UIBlockJumpTo.Arrow.color = UnclickedColor;
        }
        public void EnableArrow()
        {
            StartCoroutine(SetupConnection());
            // m_ArrowJump.EnableSelf();
        }
        IEnumerator SetupConnection()
        {
            yield return new WaitUntil(() => m_UIBlockJumpTo != null);
            m_Connection.SetTargets(TopPanel, m_UIBlockJumpTo.TopPanel);
            m_Connection.SetWeight(1, m_WeightConnectionTo);
            m_Connection.SetWeight(0, m_WeightConnectionFrom);
        }
        public void SetupArrow()
        {
            StartCoroutine(Cor_SetupArrow());
        }
        IEnumerator Cor_SetupArrow()
        {
            yield return new WaitUntil(() => m_UIBlockJumpTo != null);
            // m_ArrowJump.SetupSelf(m_UIBlockJumpTo);
        }
        public void DisableArrow()
        {
            // m_ArrowJump.DisableSelf();
        }
        void OnDestroy() 
        {
            if (m_UIBlockJumpTo == null)
            {
                return;
            }
            m_UIBlockJumpTo.DestroySelf(m_UIBlockJumpTo.gameObject);
        }
    }
}
