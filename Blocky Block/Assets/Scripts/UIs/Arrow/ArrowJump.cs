using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utility.SLayout;
using BlockyBlock.Events;

namespace BlockyBlock.UI 
{
    public class ArrowJump : MonoBehaviour
    {
        public float m_ScaleFactor {get; set;}
        public Transform m_UIBlockJumpFrom;
        public RectTransform m_ArrowStart, m_ArrowBody, m_ArrowEnd;
        public float m_ArrowStartInitLengthMin, m_ArrowStartInitLengthMax;
        float m_ArrowStartInitLength;
        void Start()
        {
            m_ArrowStartInitLength = Random.Range(m_ArrowStartInitLengthMin, m_ArrowStartInitLengthMax);
            
            GameObject ideContent = GameObject.FindGameObjectWithTag(GameConstants.IDE_CONTENT_TAG);
            SVerticalLayoutGroup contentLayout = ideContent.GetComponent<SVerticalLayoutGroup>();
            m_ScaleFactor = contentLayout.spacing;
        }
        public void EnableSelf()
        {
            gameObject.SetActive(true);
        }
        public void SetupSelf(UIBlockSkip _uiBlockJumpTo)
        {
            if (m_UIBlockJumpFrom.gameObject == null)
            {
                return;
            }
            int idxFrom = m_UIBlockJumpFrom.GetSiblingIndex();
            int idxTo = _uiBlockJumpTo.transform.GetSiblingIndex();

            int scaleY = idxFrom > idxTo ? 1 : -1;
            int idxDiff = Mathf.Abs(idxFrom - idxTo);

            float actualArrowHeight = idxDiff * m_ScaleFactor;
            transform.localScale = new Vector3(1, scaleY, 1);

            Sequence animatedSequence = DOTween.Sequence();
            animatedSequence.Append(
                m_ArrowStart
                    .DOSizeDelta(
                        new Vector2(m_ArrowStartInitLength, m_ArrowStart.sizeDelta.y),
                        0.1f
                    )
            );
            animatedSequence.Append(
                m_ArrowBody
                    .DOSizeDelta(
                        new Vector2(m_ArrowBody.sizeDelta.x, actualArrowHeight),
                        0.1f
                    )
            );
            animatedSequence.Append(
                m_ArrowEnd
                    .DOSizeDelta(
                        new Vector2(m_ArrowStartInitLength, m_ArrowStart.sizeDelta.y),
                        0.1f
                    )
            );
        }
        public void UnSetupSelf()
        {

            Sequence animatedSequence = DOTween.Sequence();
            animatedSequence.Append(
                m_ArrowEnd
                    .DOSizeDelta(
                        new Vector2(0, m_ArrowStart.sizeDelta.y),
                        0.1f
                    )
            );
            animatedSequence.Append(
                m_ArrowBody
                    .DOSizeDelta(
                        new Vector2(m_ArrowBody.sizeDelta.x, 0),
                        0.1f
                    )
            );
            animatedSequence.Append(
                m_ArrowStart
                    .DOSizeDelta(
                        new Vector2(0, m_ArrowStart.sizeDelta.y),
                        0.1f
                    )
            );
        }
        public void DisableSelf()
        {
            UnSetupSelf();
        }
    }
}
