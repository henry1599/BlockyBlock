using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.UI;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using DG.Tweening;
using Helpers;
using Helpers.BlockyBlock;

namespace BlockyBlock.UI 
{
    public class UIOptionGrid : MonoBehaviour
    {
        public GameObject m_SideRectIDE;
        public UIOptionGridDirection[] m_Items;
        public float ShowDuration;
        public virtual void Setup(bool _status, Transform _rectSnapTo)
        {
            foreach (UIOptionGridDirection item in m_Items)
            {
                item.UnsetHover();
            }
            m_SideRectIDE.SetActive(!_status);
            if (_status)
            {
                // m_ThisRect.anchoredPosition = _rectSnapTo.anchoredPosition;
                transform.position = _rectSnapTo.position;
                transform.DOScale(Vector3.one, ShowDuration).SetEase(Ease.OutBack);
            }
            else
            {
                transform.DOScale(Vector3.zero, ShowDuration).SetEase(Ease.InBack);
            }
        }
    }
}
