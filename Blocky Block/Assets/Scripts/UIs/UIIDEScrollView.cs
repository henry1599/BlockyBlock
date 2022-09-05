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
        [SerializeField] ScrollRect m_ScrollView;
        void Start()
        {
            EditorEvents.ON_IDE_SCROLL += HandleIDEScroll;
        }
        void OnDestroy()
        {
            EditorEvents.ON_IDE_SCROLL -= HandleIDEScroll;
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
