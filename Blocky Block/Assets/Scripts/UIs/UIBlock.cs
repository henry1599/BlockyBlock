using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BlockyBlock.Managers;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.UI
{
    public class UIBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected bool m_IsDragging = false;
        protected Vector3 m_DragOffset = Vector3.zero;




        [Space(10)]
        [Header("Block Visible")]
        [SerializeField] protected BlockType m_Type;
        [SerializeField] protected GameObject m_IDEObject;
        [SerializeField] protected GameObject m_PreviewObject;




        [Space(10)]
        [Header("Drag & Drop Value")]
        [SerializeField] protected CanvasGroup m_CanvasGroup;
        protected Transform m_OutsideContainerPrefab;



        UIBlock m_TempBlock = null;
        BlockMode m_Mode;
        public BlockMode Mode 
        {
            get => m_Mode;
            set
            {
                m_Mode = value;
                m_IDEObject.SetActive(m_Mode == BlockMode.IDE);
                m_PreviewObject.SetActive(m_Mode == BlockMode.PREVIEW);
            }
        }
        public BlockType Type => m_Type;
        public bool IsDragging 
        {
            get => m_IsDragging;
            set => m_IsDragging = value;
        }
        protected Transform m_CurrentContentField;
        void Awake()
        {
            m_OutsideContainerPrefab = GameObject.FindGameObjectWithTag(GameConstants.UIBLOCK_OUTSIDE_CONTAINER_TAG).transform;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_CurrentContentField = m_OutsideContainerPrefab;
            Mode = BlockMode.PREVIEW;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (m_OutsideContainerPrefab == null)
            {
                return;
            }
            if (Mode == BlockMode.PREVIEW)
            {
                m_TempBlock = Instantiate(gameObject, transform.position, Quaternion.identity, m_OutsideContainerPrefab).GetComponent<UIBlock>();
                m_TempBlock.transform.parent = null;
                m_TempBlock.transform.SetParent(m_OutsideContainerPrefab);
            }
            else
            {
                m_TempBlock = null;
                transform.parent = null;
                transform.SetParent(m_OutsideContainerPrefab);
            }
            m_IsDragging = true;
            m_DragOffset = transform.position - (Vector3)eventData.position;
            // m_CanvasGroup.blocksRaycasts = false;
        }
        public virtual void OnDrag(PointerEventData data)
        {
            if (m_OutsideContainerPrefab == null)
            {
                return;
            }
            if (m_IsDragging)
            {
                if (m_TempBlock != null)
                {
                    m_TempBlock.transform.position = (Vector3)data.position + m_DragOffset;
                }
                else
                {
                    transform.position = (Vector3)data.position + m_DragOffset;
                }
                if (UIManager.Instance.CheckTriggerUI(BlockMode.IDE, out Transform _IDEContainer))
                {
                    if (m_TempBlock != null)
                    {
                        m_TempBlock.Mode = BlockMode.IDE;
                    }
                    else
                    {
                        Mode = BlockMode.IDE;
                    }
                    m_CurrentContentField = _IDEContainer;
                }
                else if (UIManager.Instance.CheckTriggerUI(BlockMode.PREVIEW, out Transform _PreviewContainer))
                {
                    if (m_TempBlock != null)
                    {
                        m_TempBlock.Mode = BlockMode.PREVIEW;
                    }
                    else
                    {
                        Mode = BlockMode.PREVIEW;
                    }
                    m_CurrentContentField = _PreviewContainer;
                }
                else
                {
                    m_CurrentContentField = m_OutsideContainerPrefab;
                }
            }
        }
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (m_OutsideContainerPrefab == null)
            {
                return;
            }
            if (!m_IsDragging)
            {
                return;
            }
            m_IsDragging = false;
            m_DragOffset = Vector3.zero;
            // m_CanvasGroup.blocksRaycasts = true;

            if (m_TempBlock != null)
            {
                if (!m_CurrentContentField.gameObject.CompareTag("IDE Content"))
                {
                    Destroy(m_TempBlock.gameObject);
                    return;
                }
                m_TempBlock.transform.SetParent(m_CurrentContentField);
                return;
            }
            if (!m_CurrentContentField.gameObject.CompareTag("IDE Content"))
            {
                Destroy(gameObject);
                return;
            }
            transform.SetParent(m_CurrentContentField);
        }
    }
}
