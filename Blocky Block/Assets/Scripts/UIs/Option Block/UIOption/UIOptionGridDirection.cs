using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BlockyBlock.Events;

namespace BlockyBlock.UI
{
    public class UIOptionGridDirection : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] Image m_ThisImage;
        public Color m_HoverColor;
        public Color m_UnhoverColor;
        void Start()
        {
            BlockEvents.ON_UNHOVER_ALL_GRID_ITEM += UnsetHover;
        }
        void OnDestroy()
        {
            BlockEvents.ON_UNHOVER_ALL_GRID_ITEM -= UnsetHover;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UnsetHover();
        }
        public void SetHover()
        {
            m_ThisImage.color = m_HoverColor;
        }
        public void UnsetHover()
        {
            m_ThisImage.color = m_UnhoverColor;
        }
    }
}
