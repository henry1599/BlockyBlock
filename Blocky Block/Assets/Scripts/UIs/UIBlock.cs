using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
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
        [Header("Raycast Target Children")]
        [SerializeField] List<Image> m_ChildrenImage;




        [Space(10)]
        [Header("Drag & Drop Value")]
        [SerializeField] protected CanvasGroup m_CanvasGroup;
        protected Transform m_OutsideContainerPrefab;



        UIBlock m_TempBlock = null;
        BlockMode m_Mode;
        public CanvasGroup CanvasGroup => m_CanvasGroup;
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
            if (Mode == BlockMode.PREVIEW)
            {
                InitTempBlock();
            }
            else
            {
                InitBlock();
            }
            m_DragOffset = transform.position - (Vector3)eventData.position;
            m_IsDragging = true;
            m_CanvasGroup.blocksRaycasts = false;
            ToggleChildrenRaycastTarget(false);
        }
        void InitTempBlock()
        {
            m_TempBlock = Instantiate(gameObject, transform.position, Quaternion.identity, m_OutsideContainerPrefab).GetComponent<UIBlock>();
            m_TempBlock.transform.parent = null;
            m_TempBlock.transform.SetParent(m_OutsideContainerPrefab);
            m_TempBlock.IsDragging = true;
            m_TempBlock.CanvasGroup.blocksRaycasts = false;
            m_TempBlock.ToggleChildrenRaycastTarget(false);
        }
        void InitBlock()
        {
            m_TempBlock = null;
            transform.parent = null;
            transform.SetParent(m_OutsideContainerPrefab);
        }
        public virtual void OnDrag(PointerEventData data)
        {
            if (m_IsDragging)
            {
                CastBlockPosition();
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
        public virtual void CastBlockPosition()
        {
            if (UIManager.Instance.CheckTriggerUI(BlockMode.BLOCK_ON_BLOCK, out Transform _BlockTransform))
            {
                if (transform != _BlockTransform)
                {
                    int nextIdx = _BlockTransform.GetSiblingIndex();
                    UIManager.Instance.m_DummyUIBlock.transform.SetSiblingIndex(nextIdx);
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
            m_CanvasGroup.blocksRaycasts = true;
            ToggleChildrenRaycastTarget(true);

            if (m_TempBlock != null)
            {
                m_TempBlock.IsDragging = false;
                m_TempBlock.CanvasGroup.blocksRaycasts = true;
                m_TempBlock.ToggleChildrenRaycastTarget(true);
                if (!m_CurrentContentField.gameObject.CompareTag("IDE Content"))
                {
                    UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
                    DestroySelf(m_TempBlock.transform);
                    return;
                }
                m_TempBlock.transform.SetParent(m_CurrentContentField);
                m_TempBlock.transform.SetSiblingIndex(UIManager.Instance.m_DummyUIBlock.GetSiblingIndex());
                UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
                return;
            }
            if (!m_CurrentContentField.gameObject.CompareTag("IDE Content"))
            {
                UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
                DestroySelf(transform);
                return;
            }
            transform.SetParent(m_CurrentContentField);
            transform.SetSiblingIndex(UIManager.Instance.m_DummyUIBlock.GetSiblingIndex());
            UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
        }
        public virtual void DestroySelf(Transform _target)
        {
            _target
                .DOScale(
                    Vector3.zero,
                    0.25f
                )
                .OnComplete(() => Destroy(_target.gameObject));
        }
        public void ToggleChildrenRaycastTarget(bool _status)
        {
            foreach (Image img in m_ChildrenImage)
            {
                img.raycastTarget = _status;
            }
        }
    }
}
