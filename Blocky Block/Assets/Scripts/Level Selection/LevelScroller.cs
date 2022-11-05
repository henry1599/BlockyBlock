using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlockyBlock.Managers;
using BlockyBlock.Events;

namespace BlockyBlock
{
    public class LevelScroller : MonoBehaviour
    {
        [SerializeField] Transform m_ObjectToScroll;
        [SerializeField] float m_ScrollSpeed;
        [SerializeField] LayerMask m_LevelItemMask;
        private float m_SnapXOffset;
        private bool m_IsDragging;
        private Vector2 m_PressedPosition;
        private Camera m_Cam;
        private Vector2 m_MouseLastFrame;
        private float m_MinScrollX = 0;
        private float m_MaxScrollX;
        private bool m_IsSnapping = false;
        public bool CanScroll = true;
        void Awake()
        {
            StartCoroutine(Cor_GetOffset());
        }
        void Start()
        {
            m_Cam = Camera.main;
            LevelSelectionEvents.ON_ITEM_HOVER += HandleItemHover;
        }
        void OnDestroy()
        {
            LevelSelectionEvents.ON_ITEM_HOVER -= HandleItemHover;
        }
        void HandleItemHover(bool _status)
        {
            CanScroll = !_status;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_MouseLastFrame = Input.mousePosition;
                m_IsDragging = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (m_IsDragging)
                {
                    m_IsDragging = false;
                    m_PressedPosition = Vector2.zero;
                }
            }


            if (CanScroll)
            {
                if (m_IsDragging && !m_IsSnapping)
                {
                    float scrollDelta = ((Vector2)Input.mousePosition - m_MouseLastFrame).x;
                    Vector3 scrollPosition = m_ObjectToScroll.position;
                    
                    scrollPosition.x -= scrollDelta * m_ScrollSpeed * Time.deltaTime;
                    scrollPosition.x = Mathf.Clamp(scrollPosition.x, m_MinScrollX, m_MaxScrollX);
                    m_ObjectToScroll.position = scrollPosition;
                    m_MouseLastFrame = Input.mousePosition;
                }
            }
            if (!m_IsSnapping)
                HighlightItem();
        }
        IEnumerator Cor_GetOffset()
        {
            yield return new WaitUntil(() => LevelMenuManager.Instance != null);
            m_SnapXOffset = LevelMenuManager.Instance.ScrollXOffset;
            m_MinScrollX = 0;
            m_MaxScrollX = (LevelMenuManager.Instance.ItemCount - 1) * m_SnapXOffset;
        }
        void HighlightItem()
        {
            Ray ray = new Ray(m_Cam.transform.position, m_Cam.transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, m_LevelItemMask, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.collider.gameObject != null)
                {
                    LevelSelectionEvents.ON_HIGHLIGHT_ITEM?.Invoke(hitInfo.collider.gameObject.GetComponent<LevelItem>());
                }
            }
            else
            {
                LevelSelectionEvents.ON_HIGHLIGHT_ITEM?.Invoke(null);
            }
        }
        public void SnapTo(LevelItem _item)
        {
            // m_IsDragging = false;
            m_IsSnapping = true;
            m_ObjectToScroll
                .DOMoveX(
                    _item.transform.position.x,
                    0.5f
                )
                .OnComplete(() => m_IsSnapping = false);
        }
        void OnDrawGizmos()
        {
            if (m_Cam != null)
                Gizmos.DrawRay(new Ray(m_Cam.transform.position, m_Cam.transform.forward * 100000));
        }
    }
}
