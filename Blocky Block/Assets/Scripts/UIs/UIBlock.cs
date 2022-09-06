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
        [SerializeField] protected RectTransform m_ThisRect;
        [SerializeField] protected BlockType m_Type;
        [SerializeField] protected GameObject m_IDEObject;
        [SerializeField] protected GameObject m_PreviewObject;




        [Space(10)]
        [Header("Highlight value")]
        [SerializeField] protected UICompilerSignal m_Signal;




        [Space(10)]
        [Header("Raycast Target Children")]
        [SerializeField] List<Image> m_ChildrenImage;




        [Space(10)]
        [Header("Drag & Drop Value")]
        [SerializeField] protected CanvasGroup m_CanvasGroup;
        protected Transform m_OutsideContainerPrefab;
        public UILineNumber m_UILineNumber;



        protected ScrollRect m_ScrollRect;
        protected RectTransform m_ContentPanel;
        UIBlock m_TempBlock = null;
        BlockMode m_Mode;
        ScrollIDEState ScrollIDEState
        {
            get => m_ScrollIDEState;
            set 
            {
                if (m_ScrollIDEState == value)
                {
                    return;
                }
                m_ScrollIDEState = value;
                EditorEvents.ON_IDE_SCROLL?.Invoke(value);
            }
        } ScrollIDEState m_ScrollIDEState;
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
            m_ScrollRect = GameObject.FindGameObjectWithTag(GameConstants.IDE_SCROLL_RECT_TAG).GetComponent<ScrollRect>();
            m_ContentPanel = GameObject.FindGameObjectWithTag(GameConstants.IDE_CONTENT_TAG).GetComponent<RectTransform>();
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {
            m_CurrentContentField = m_OutsideContainerPrefab;
            Mode = BlockMode.PREVIEW;

            BlockEvents.ON_HIGHLIGHT += HandleHighlight;
        }
        void Update()
        {
            // CastBlockPosition();
        }
        void OnDestroy()
        {
            BlockEvents.ON_HIGHLIGHT -= HandleHighlight;
        }
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            m_DragOffset = new Vector3(-210, 0, 0);

            if (Mode == BlockMode.PREVIEW)
            {
                InitTempBlock(eventData);
            }
            else
            {
                InitBlock();
            }

            // m_DragOffset = transform.position - (Vector3)eventData.position;
            m_IsDragging = true;
            m_CanvasGroup.blocksRaycasts = false;
            ToggleChildrenRaycastTarget(false);
        }
        void InitTempBlock(PointerEventData eventData)
        {
            m_TempBlock = Instantiate(gameObject, (Vector3)eventData.position + m_DragOffset, Quaternion.identity, m_OutsideContainerPrefab).GetComponent<UIBlock>();

            m_TempBlock.CanvasGroup.alpha = 0;

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
                Cursor.visible = false;
                CastBlockPosition();
                CastIDETopDown();
                if (m_TempBlock != null)
                {
                    m_TempBlock.transform.position = (Vector3)data.position + m_DragOffset;
                    
                    StartCoroutine(SetDelay(() => m_TempBlock.CanvasGroup.alpha = 1, 0.05f));
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
            // else if (!UIManager.Instance.CheckTriggerUI(BlockMode.DUMMY_BLOCK, out Transform _DummyBlockTransform))
            // {
            //     UIManager.Instance.m_DummyUIBlock.transform.SetAsLastSibling();
            // }
        }
        public virtual void CastIDETopDown()
        {
            if (UIManager.Instance.CheckTriggerUI(GameConstants.TOP_IDE_TAG))
            {
                print("TOP");
                ScrollIDEState = ScrollIDEState.SCROLL_DOWN;
                UIManager.Instance.m_DummyUIBlock.transform.SetAsLastSibling();
            }
            else if (UIManager.Instance.CheckTriggerUI(GameConstants.BELOW_IDE_TAG))
            {
                print("BELOW");
                ScrollIDEState = ScrollIDEState.SCROLL_UP;
                UIManager.Instance.m_DummyUIBlock.transform.SetAsLastSibling();
            }
            else
            {
                ScrollIDEState = ScrollIDEState.STOP_SCROLLING;
            }
        }
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            Cursor.visible = true;
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
        IEnumerator SetDelay(System.Action _cb = null, float _delay = 0.0f)
        {
            yield return new WaitForSeconds(_delay);
            _cb?.Invoke();
        }
        public void ToggleChildrenRaycastTarget(bool _status)
        {
            foreach (Image img in m_ChildrenImage)
            {
                img.raycastTarget = _status;
            }
        }
        void HandleHighlight(UIBlock _uiBlock, IDERunState _state)
        {
            if (this == _uiBlock)
            {
                HighlightSelf(_state);
            }
            else
            {
                UnHighlightSelf(_state);
            }
        }
        public virtual void HighlightSelf(IDERunState _state)
        {
            SnapTo(m_ThisRect);
            switch (_state)
            {
                case IDERunState.MANNUAL:
                    m_Signal?.TogglePlayMode();
                    break;
                case IDERunState.DEBUGGING:
                    m_Signal?.ToggleDebugMode();
                    break;
                default:
                    break;
            }
        }
        public virtual void UnHighlightSelf(IDERunState _state)
        {
            m_Signal?.DisableSelf();
        }
        public virtual void Setup(UIBlock _parentBlock = null)
        {
        }
        public virtual void UpdateLineNumber()
        {
            m_UILineNumber.Setup(transform);
        }
        public void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = (Vector2)m_ScrollRect.transform.InverseTransformPoint(m_ContentPanel.position) - (Vector2)m_ScrollRect.transform.InverseTransformPoint(target.position) - new Vector2(0, 100);
            m_ContentPanel
                .DOAnchorPos(
                    newPosition,
                    1
                )
                .SetEase(Ease.InOutSine);
                    
        }
    }
}
