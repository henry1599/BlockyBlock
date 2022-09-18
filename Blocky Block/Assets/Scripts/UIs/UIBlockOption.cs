using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using BlockyBlock.Events;
using TMPro;

namespace BlockyBlock.UI 
{
    public class UIBlockOption : MonoBehaviour, IPointerClickHandler
    {
        public Transform m_TopPanel, m_BelowPanel;
        public GameObject m_TextObject;
        public UIBlock UIBlock;
        bool m_Status = false;
        public void OnPointerClick(PointerEventData eventData)
        {
            m_Status = !m_Status;
            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED?.Invoke(this, m_Status);
        }

        public virtual void Setup()
        {
            m_TopPanel
                .DOScaleX(
                    1,
                    0.25f
                )
                .SetEase(Ease.InOutSine);
            m_BelowPanel
                .DOScaleX(
                    1,
                    0.25f
                )
                .SetEase(Ease.InOutSine)
                .OnComplete(() => m_TextObject.SetActive(true));
        }
    }
}
