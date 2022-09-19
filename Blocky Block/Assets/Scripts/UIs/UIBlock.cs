using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using BlockyBlock.Managers;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.Tools;
using Helpers;
using TMPro;

namespace BlockyBlock.UI
{
    public class UIBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler 
    {
        protected bool m_IsDragging = false;
        protected Vector3 m_DragOffset = Vector3.zero;




        [Space(10)]
        [Header("Block Init")]
        [SerializeField] protected Image m_TopPanelIDE;
        [SerializeField] protected Image m_BelowPanelIDE;
        [SerializeField] protected Image m_TopPanelPreview;
        [SerializeField] protected Image m_BelowPanelPreview;
        [SerializeField] protected TMP_Text m_TextIDE;
        [SerializeField] protected TMP_Text m_TextPreview;





        
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
        public Vector3 m_PivotOffset;
        [SerializeField] protected CanvasGroup m_CanvasGroup;
        protected Transform m_OutsideContainerPrefab;
        public UILineNumber m_UILineNumber;




        [Space(10)]
        [Header("Optional Block")]
        public UIBlockOption UIBlockOption;




        // [Space(10)]
        // [Header("Click Shake Rotation")]
        // [Range(0.05f, 1f)]
        // public float ShakeDuration = 0.15f;
        // public Vector3 ShakeStrength = new Vector3(0, 0, 1);
        // [Range(0f, 180f)]
        // public float Randomness = 90f;
        // [Range(10f, 100f)]
        // public float Vibrato = 10;



        protected bool m_IsDisabled = false;
        protected RectTransform m_IDEMainField;
        protected RectTransform m_ContentPanel;
        protected Color m_TopColor, m_BelowColor;
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
        private bool m_IsHoverOptionBlock;
        private bool m_IsDragOnOptionBlock;
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
        public virtual bool IsDragging 
        {
            get => m_IsDragging;
            set => m_IsDragging = value;
        }
        protected Transform m_CurrentContentField;
        private Vector3 m_InitClickPosition;
        void Awake()
        {
            m_OutsideContainerPrefab = GameObject.FindGameObjectWithTag(GameConstants.UIBLOCK_OUTSIDE_CONTAINER_TAG).transform;
            m_ContentPanel = GameObject.FindGameObjectWithTag(GameConstants.IDE_CONTENT_TAG).GetComponent<RectTransform>();
            m_IDEMainField = GameObject.FindGameObjectWithTag(GameConstants.IDE_MAIN_FIELD_TAG).GetComponent<RectTransform>();
        }
        public void Init(Color _topColor, Color _belowColor, string _text)
        {
            m_TopColor = _topColor;
            m_BelowColor = _belowColor;
            m_TextIDE.text = m_TextPreview.text = _text;
            m_TopPanelIDE.color = m_TopPanelPreview.color = _topColor;
            m_BelowPanelIDE.color = m_BelowPanelPreview.color = _belowColor;
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
            if (UIManager.Instance.CheckTriggerUI(GameConstants.UI_BLOCK_OPTION_TAG))
            {
                m_IsHoverOptionBlock = true;
            }
            else
            {
                m_IsHoverOptionBlock = false;
            }
            if (m_IsDragging)
            {
                CastBlockPosition();
            }
            if (UIManager.Instance.m_DelayBufferTimer >= 0)
            {
                UIManager.Instance.m_DelayBufferTimer -= Time.deltaTime;
            }
        }
        void OnDestroy()
        {
            BlockEvents.ON_HIGHLIGHT -= HandleHighlight;
            BlockEvents.BLOCK_IDE_UI -= HanldeBlockIDEUI;
            BlockEvents.UNBLOCK_IDE_UI -= HanldeUnBlockIDEUI;
        }
        void HanldeBlockIDEUI(UIBlock _uiBlockCallFrom)
        {
            if (_uiBlockCallFrom != this)
            {
                CanvasGroup 
                    .DOFade(
                        0.4f,
                        0.2f
                    );
                if (UIBlockOption == null)
                {
                    m_IsDisabled = true;
                }
                else 
                {
                    UIBlockOption.IsDisabled = m_IsDisabled = true;
                }
            }
            // ToggleChildrenRaycastTarget(false);
        }
        void HanldeUnBlockIDEUI(UIBlock _uiBlockCallFrom)
        {
            if (_uiBlockCallFrom != this)
            {
                CanvasGroup 
                    .DOFade(
                        1f,
                        0.2f
                    );
                if (UIBlockOption == null)
                {
                    m_IsDisabled = false;
                }
                else 
                {
                    UIBlockOption.IsDisabled = m_IsDisabled = false;
                }
            }
            // ToggleChildrenRaycastTarget(true);
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (Mode == BlockMode.PREVIEW)
            {
                BlockEvents.ON_DISABLE_UI_FUNCTION?.Invoke();
                return;
            }
            if (m_IsHoverOptionBlock)
            {
                m_IsDragOnOptionBlock = true;
                return;
            }
            ClickSelf();
            Vector3 endPosition = (Vector3)(pointerEventData.position) + m_PivotOffset;

            m_InitClickPosition = transform.position;
            transform
                .DOMove(endPosition, 0.1f).SetEase(Ease.InOutSine);
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            if (Mode == BlockMode.PREVIEW)
            {
                return;
            }
            if (m_IsDragOnOptionBlock)
            {
                m_IsDragOnOptionBlock = false;
                return;
            }
            UnclickSelf();
            if (!m_IsDragging)
            {
                transform.DOKill();
                transform.position = m_InitClickPosition;
            }
            
        }
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (HandToolManager.Instance.CurrentCursor != CursorType.SELECTION)
            {
                return;
            }
            if (m_IsDragOnOptionBlock)
            {
                return;
            }
            BlockEvents.ON_UI_BLOCK_DRAG?.Invoke(true);
            m_DragOffset = m_PivotOffset;

            if (Mode == BlockMode.PREVIEW)
            {
                InitTempBlock(eventData);
            }
            else
            {
                InitBlock(eventData);
            }

            // m_DragOffset = transform.position - (Vector3)eventData.position;
            m_IsDragging = true;
            m_CanvasGroup.blocksRaycasts = false;
            ToggleChildrenRaycastTarget(false);
        }
        void InitTempBlock(PointerEventData eventData)
        {
            m_TempBlock = Instantiate(gameObject, (Vector3)(eventData.position) + m_DragOffset, Quaternion.identity, m_OutsideContainerPrefab).GetComponent<UIBlock>();
            // m_TempBlock = Instantiate(gameObject).GetComponent<UIBlock>();

            m_TempBlock.CanvasGroup.alpha = 0;

            m_TempBlock.transform.SetParent(null);
            m_TempBlock.transform.SetParent(m_OutsideContainerPrefab);

            m_TempBlock.IsDragging = true;
            m_TempBlock.CanvasGroup.blocksRaycasts = false;
            m_TempBlock.ToggleChildrenRaycastTarget(false);
            m_TempBlock.m_UILineNumber.Unset();

        }
        void InitBlock(PointerEventData _data)
        {
            m_TempBlock = null;
            transform.SetParent(null);
            transform.SetParent(m_OutsideContainerPrefab);
            // transform
            //     .DOMove(
            //         (Vector3)_data.position + m_DragOffset,
            //         0.15f
            //     );
            m_UILineNumber.Unset();
        }
        public virtual void OnDrag(PointerEventData data)
        {
            if (HandToolManager.Instance.CurrentCursor != CursorType.SELECTION)
            {
                return;
            }
            if (m_IsDragOnOptionBlock)
            {
                return;
            }
            if (m_IsDragging)
            {
                // Cursor.visible = false;
                CastBlockPosition();
                CastIDETopDown();
                if (m_TempBlock != null)
                {
                    m_TempBlock.transform.DOKill();
                    m_TempBlock.transform.position = (Vector3)(data.position) + m_DragOffset;
                    
                    StartCoroutine(SetDelay(() => m_TempBlock.CanvasGroup.alpha = 1, 0.05f));
                    m_TempBlock.m_UILineNumber.Unset();
                    m_TempBlock.Mode = BlockMode.IDE;
                }
                else
                {
                    transform.DOKill();
                    transform.position = (Vector3)(data.position) + m_DragOffset;

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
                    if (UIManager.Instance.m_DelayBufferTimer < 0 && UIManager.Instance.m_IsTweening == false)
                    {
                        int nextIdx = _BlockTransform.GetSiblingIndex();
                        UIManager.Instance.m_DummyUIBlock.transform.SetSiblingIndex(nextIdx);
                        UIManager.Instance.m_DelayBufferTimer = UIManager.Instance.m_DelayBuffer;
                    }
                }
            }
            else if (UIManager.Instance.CheckTriggerUI(GameConstants.NOT_ANY_BLOCK_TAG))
            {
                if (UIManager.Instance.m_DelayBufferTimer < 0 && UIManager.Instance.m_IsTweening == false)
                {
                    UIManager.Instance.m_DummyUIBlock.transform.SetAsLastSibling();
                    UIManager.Instance.m_DelayBufferTimer = UIManager.Instance.m_DelayBuffer;
                }
            }
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
            if (HandToolManager.Instance.CurrentCursor != CursorType.SELECTION)
            {
                return;
            }
            if (m_IsDragOnOptionBlock)
            {
                m_IsDragOnOptionBlock = false;
                return;
            }
            BlockEvents.ON_UI_BLOCK_DRAG?.Invoke(false);
            UIManager.Instance.m_DelayBufferTimer = UIManager.Instance.m_DelayBuffer;
            // Cursor.visible = true;
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
                    DestroySelf(m_TempBlock.gameObject);
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
                DestroySelf(gameObject);
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
        public virtual void DestroySelf(GameObject _target)
        {
            _target.transform
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
            try
            {
                SnapTo(transform);
            }
            catch (System.Exception)
            {
            }
            try
            {
                UIManager.Instance.CurrentIdx = transform.GetSiblingIndex();
            }
            catch (System.Exception)
            {
            }
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
        public virtual void ClickSelf()
        {
            BlockEvents.ON_DISABLE_UI_FUNCTION?.Invoke();
        }
        public virtual void UnclickSelf()
        {

        }
        public virtual void UnHighlightSelf(IDERunState _state)
        {
            m_Signal?.DisableSelf();
        }
        public virtual void Setup(UIBlock _parentBlock = null)
        {
            BlockEvents.BLOCK_IDE_UI += HanldeBlockIDEUI;
            BlockEvents.UNBLOCK_IDE_UI += HanldeUnBlockIDEUI;
        }
        public virtual void UpdateLineNumber()
        {
            m_UILineNumber.Setup(transform);
        }
        public void SnapTo(Transform target)
        {
            if (m_ContentPanel == null || target == null)
            {
                return;
            }
            if (target.GetSiblingIndex() < UIManager.Instance.ScrollBlockIdx)
            {
                EditorEvents.ON_IDE_SCROLL?.Invoke(ScrollIDEState.SCROLL_DOWN);
                return;
            }
            Canvas.ForceUpdateCanvases();
            int thisIdx = transform.GetSiblingIndex();
            int lastIdx = UIManager.Instance.CurrentIdx;
            int blockDiff = thisIdx - lastIdx;
            float scrollDelta = 50.0f * blockDiff;
            // print(scrollDelta);
            EditorEvents.ON_IDE_SCROLL_SNAP?.Invoke(scrollDelta);
        }
    }
}
