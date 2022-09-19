using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using DG.Tweening;
using UnityEngine.UI;

namespace BlockyBlock.UI 
{
    public class UIIDEScrollView : MonoBehaviour
    {
        [SerializeField] RectTransform m_ThisRect;
        [SerializeField] ScrollRect m_ScrollView;
        [SerializeField] RectTransform m_Content;
        [SerializeField] GameObject m_TopScrollIDE, m_BottomScrollIDE;
        Sequence m_TweenSequence;
        void Start()
        {
            m_TweenSequence = DOTween.Sequence();

            m_ThisRect.sizeDelta = new Vector2(m_ThisRect.sizeDelta.x, Screen.height);

            EditorEvents.ON_IDE_SCROLL += HandleIDEScroll;
            EditorEvents.ON_IDE_SCROLL_SNAP += HandleIDEScrollSnap;

            UnitEvents.ON_RESET += HandleResetIDE;
            GameEvents.ON_EXECUTING_BLOCK += HandleExecutingBlock;

            BlockEvents.ON_UI_BLOCK_DRAG += HandleUIBlockDrag;
        }
        void OnDestroy()
        {
            EditorEvents.ON_IDE_SCROLL -= HandleIDEScroll;
            EditorEvents.ON_IDE_SCROLL_SNAP -= HandleIDEScrollSnap;
            
            UnitEvents.ON_RESET -= HandleResetIDE;
            GameEvents.ON_EXECUTING_BLOCK -= HandleExecutingBlock;
            
            BlockEvents.ON_UI_BLOCK_DRAG -= HandleUIBlockDrag;
        }
        void HandleUIBlockDrag(bool _status)
        {
            m_TopScrollIDE.SetActive(_status);
            m_BottomScrollIDE.SetActive(_status);
        }
        void HandleExecutingBlock(bool _isExecuting)
        {
            m_ScrollView.enabled = !_isExecuting;
        }
        void HandleResetIDE()
        {
            m_TweenSequence = DOTween.Sequence();
            DOTween.Kill(m_Content);
        }
        void HandleIDEScrollSnap(float _delta)
        {
            float maxHeight = m_Content.sizeDelta.y - 1080;
            Vector2 curPos = m_Content.anchoredPosition;
            curPos.y += _delta;
            if (curPos.y > maxHeight) curPos.y = maxHeight;

            Tween tween = m_Content
                .DOAnchorPos(
                    curPos,
                    1f
                )
                .SetEase(Ease.InOutSine);
            m_TweenSequence.Append(tween);
            // if (m_ScrollView.normalizedPosition.y <= 0)
            // {
            //     m_ScrollView.normalizedPosition = new Vector2(m_ScrollView.normalizedPosition.x, 0);
            //     return;
            // }
        }
        void HandleIDEScroll(ScrollIDEState _state)
        {
            switch (_state)
            {
                case ScrollIDEState.SCROLL_UP:
                    m_ScrollView
                        .DOVerticalNormalizedPos(
                            0,
                            1f
                        )
                        .SetEase(Ease.InOutSine);
                    break;
                case ScrollIDEState.SCROLL_DOWN:
                    m_ScrollView
                        .DOVerticalNormalizedPos(
                            1,
                            1f
                        )
                        .SetEase(Ease.InOutSine);
                    break;
                case ScrollIDEState.STOP_SCROLLING:
                    m_ScrollView.DOKill();
                    break;
            }
        }
    }
}
