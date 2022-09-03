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
        public UILineNumber m_UILineNumber;



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
        protected virtual void Start()
        {
            m_CurrentContentField = m_OutsideContainerPrefab;
            Mode = BlockMode.PREVIEW;
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
            m_TempBlock.m_UILineNumber.Unset();
        }
        void InitBlock()
        {
            m_TempBlock = null;
            transform.parent = null;
            transform.SetParent(m_OutsideContainerPrefab);
            m_UILineNumber.Unset();
        }
        public virtual void OnDrag(PointerEventData data)
        {
            if (m_IsDragging)
            {
                CastBlockPosition();
                if (m_TempBlock != null)
                {
                    m_TempBlock.transform.position = (Vector3)data.position + m_DragOffset;

                    m_TempBlock.m_UILineNumber.Unset();
                    m_TempBlock.Mode = BlockMode.IDE;
                }
                else
                {
                    transform.position = (Vector3)data.position + m_DragOffset;

                    m_UILineNumber.Unset();
                    Mode = BlockMode.IDE;
                }
                if (UIManager.Instance.CheckTriggerUI(BlockMode.IDE, out Transform _IDEContainer))
                {
                    m_CurrentContentField = _IDEContainer;
                }
                else if (UIManager.Instance.CheckTriggerUI(BlockMode.PREVIEW, out Transform _PreviewContainer))
                {
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
            else
            {
                UIManager.Instance.m_DummyUIBlock.transform.SetAsLastSibling();
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
            ResetPreset();

            if (m_TempBlock != null)
            {
                m_TempBlock.IsDragging = false;
                m_TempBlock.CanvasGroup.blocksRaycasts = true;
                m_TempBlock.ToggleChildrenRaycastTarget(true);

                // * Destroy block that is not dragged into IDE
                if (!m_CurrentContentField.gameObject.CompareTag(GameConstants.IDE_CONTENT_TAG))
                {
                    UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
                    DestroySelf(m_TempBlock.transform);
                    return;
                }

                // * Drop block into IDE
                m_TempBlock.transform.SetParent(m_CurrentContentField);
                m_TempBlock.transform.GetComponent<RectTransform>().anchoredPosition = UIManager.Instance.m_DummyUIBlock.GetComponent<RectTransform>().anchoredPosition;
                m_TempBlock.transform.SetSiblingIndex(UIManager.Instance.m_DummyUIBlock.GetSiblingIndex());
                
                // * Setup Block
                m_TempBlock.Setup();
                UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
                return;
            }

            // * Destroy block that is not dragged into IDE
            if (!m_CurrentContentField.gameObject.CompareTag(GameConstants.IDE_CONTENT_TAG))
            {
                UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
                DestroySelf(transform);
                return;
            }

            // * Drop block into IDE
            transform.SetParent(m_CurrentContentField);
            transform.SetSiblingIndex(UIManager.Instance.m_DummyUIBlock.GetSiblingIndex());

            // * Setup Block
            Setup();
            UIManager.Instance.m_DummyUIBlock.SetAsLastSibling();
        }
        public virtual void ResetPreset()
        {
            m_IsDragging = false;
            m_DragOffset = Vector3.zero;
            m_CanvasGroup.blocksRaycasts = true;
            ToggleChildrenRaycastTarget(true);
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
        public void HighlightSelf()
        {
            
        }
        public virtual void Setup(UIBlock _parentBlock = null)
        {
        }
        public virtual void UpdateLineNumber()
        {
            m_UILineNumber.Setup(transform);
        }
    }
}
