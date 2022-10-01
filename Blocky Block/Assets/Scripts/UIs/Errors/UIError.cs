using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace BlockyBlock.UI 
{
    public class UIError : MonoBehaviour
    {
        [SerializeField] TMP_Text m_Text;
        [SerializeField] Transform m_BelowPanel, m_TopPanel;
        public void Open(string _msg)
        {
            m_Text.text = _msg;
            m_BelowPanel
                .DOScale(
                    Vector3.one,
                    0.5f
                )
                .SetEase(Ease.OutBack);
            m_TopPanel
                .DOScale(
                    Vector3.one,
                    0.5f
                )
                .SetEase(Ease.OutBack);
        }
        public void Close()
        {
            m_BelowPanel
                .DOScale(
                    Vector3.zero,
                    0.5f
                )
                .SetEase(Ease.InBack);
            m_TopPanel
                .DOScale(
                    Vector3.zero,
                    0.5f
                )
                .SetEase(Ease.InBack);
        }
    }
}
