using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BlockyBlock.Managers;
using BlockyBlock.Enums;

namespace BlockyBlock.UI
{
    public class UIBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        bool m_IsDragging = false;
        Vector3 m_DragOffset = Vector3.zero;
        [SerializeField] CanvasGroup m_CanvasGroup;
        [SerializeField] Transform m_OutsideContainerPrefab;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_IsDragging = true;
            m_DragOffset = transform.position - (Vector3)eventData.position;
            m_CanvasGroup.blocksRaycasts = false;
            transform.parent = null;
            transform.SetParent(m_OutsideContainerPrefab);
        }
        public void OnDrag(PointerEventData data)
        {
            if (m_IsDragging)
            {
                transform.position = (Vector3)data.position + m_DragOffset;
                if (UIManager.Instance.CheckTriggerUI(BlockMode.IDE))
                {
                    Debug.Log("Drag on IDE");
                }
                else if (UIManager.Instance.CheckTriggerUI(BlockMode.PREVIEW))
                {
                    Debug.Log("Drag on PREVIEW");
                }
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            m_IsDragging = false;
            m_DragOffset = Vector3.zero;
            m_CanvasGroup.blocksRaycasts = true;
        }
    }
}
