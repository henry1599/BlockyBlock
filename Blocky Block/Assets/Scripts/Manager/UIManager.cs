using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Enums;
using UnityEngine.EventSystems;
using BlockyBlock.Events;
using BlockyBlock.Configurations;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.UI;
using NaughtyAttributes;
using BlockyBlock.Tracking;

namespace BlockyBlock.Managers
{
    [System.Serializable]
    public class BlockData : SerializableDictionaryBase<BlockType, UIBlock> {}
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance {get; private set;}
        public GraphicRaycaster m_Raycaster;
        public GameObject[] m_BlockUIs;
        public int ScrollBlockIdx = 8;
        public GameObject m_IDECodeField;
        public Transform m_IDECodeContent;
        public GameObject m_PreviewCodeField;
        public GameObject SideRect;
        public Transform m_PreviewCodeContent;
        public EventSystem m_EventSystem;
        public BlockData m_BlockDatas;
        public Transform m_DummyUIBlock;
        public UIOption UIOptionTurn;
        public UIOption UIOptionSthFront;
        public UIOptionGrid UIOptionGrid;
        public float m_DelayBuffer = 0.15f;
        public float m_DelayBufferTimer {get; set;}
        public bool m_IsTweening = false;
        public int CurrentIdx = 0;
        PointerEventData m_PointerEventData;
        void Awake()
        {
            Instance = this;

            GameEvents.SETUP_LEVEL += HandleSetupLevel;
        }
        [Button("Show Screen Size")]
        public void ShowScreenSize()
        {
            Debug.Log("SCREEN WIDTH = " + Screen.width + ", SCREEN HEIGHT = " + Screen.height);
        }
        // Start is called before the first frame update
        void Start()
        {
            m_DelayBufferTimer = m_DelayBuffer;
            GameEvents.ON_CLEAR_IDE += HandleClearIDE;
            GameEvents.ON_WIN += HandleWin;
            GameEvents.ON_LOSE += HandleLose;
            
            EditorEvents.ON_BLOCK_EDITOR += HandleBlockEditor;
        }
        void OnDestroy()
        {
            LevelManager.Instance.SetTimeSpent();
            LevelManager.Instance.SetIsProgress(false);
            TrackingManager.Instance.StopRecord(RecordDataType.LEVEL_FINISHED);
            GameEvents.SETUP_LEVEL -= HandleSetupLevel;
            GameEvents.ON_CLEAR_IDE -= HandleClearIDE;
            GameEvents.ON_WIN -= HandleWin;
            GameEvents.ON_LOSE -= HandleLose;
            
            EditorEvents.ON_BLOCK_EDITOR -= HandleBlockEditor;
        }
        void HandleClearIDE()
        {
            foreach (Transform child in m_IDECodeContent)
            {
                if (child.GetComponent<UIBlock>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
            foreach (Transform child in m_PreviewCodeContent)
            {
                if (child.GetComponent<UIBlock>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        void HandleWin()
        {
            CountBlocksUse();
            HandleBlockEditor(true);
        }
        void HandleLose()
        {
            HandleBlockEditor(true);
        }
        void HandleBlockEditor(bool _status)
        {
            foreach (GameObject blockEditor in m_BlockUIs)
            {
                blockEditor.SetActive(_status);
            }
        }
        void HandleSetupLevel(LevelData _data)
        {
            foreach (BlockType t in _data.BlockTypes)
            {
                GameObject uiBlockObject = Instantiate(m_BlockDatas[t].gameObject);
                uiBlockObject.GetComponent<UIBlock>().Init(ConfigManager.Instance.BlockConfig.Blocks[t].TopColor, ConfigManager.Instance.BlockConfig.Blocks[t].ShadowColor, ConfigManager.Instance.BlockConfig.Blocks[t].Text);
                uiBlockObject.transform.SetParent(m_PreviewCodeContent);
                uiBlockObject.transform.localScale = Vector3.one;
            }
            
        }

        public bool CheckTriggerUI(string _objectTag)
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag(_objectTag))
                {
                    return true;
                }
                if (result.gameObject.tag == _objectTag)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckTriggerUI(BlockMode _mode, out Transform _container)
        {
            string containerName = m_IDECodeField.name;
            _container = m_IDECodeField.transform;
            switch (_mode)
            {
                case BlockMode.IDE:
                    _container = m_IDECodeContent;
                    containerName = m_IDECodeField.name;
                    break;
                case BlockMode.PREVIEW:
                    _container = m_PreviewCodeContent;
                    containerName = m_PreviewCodeField.name;
                    break;
                default:
                    _container = null;
                    containerName = string.Empty;
                    break;
            }
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag(GameConstants.UI_BLOCK_TAG))
                {
                    if (_mode == BlockMode.BLOCK_ON_BLOCK)
                    {
                        if (result.gameObject.GetComponent<UIRaycastTargetable>() == null)
                        {
                            return false;
                        }
                        _container = result.gameObject.GetComponent<UIRaycastTargetable>().Root;
                        return true;
                    }
                }
                if (_mode == BlockMode.DUMMY_BLOCK)
                {
                    if (result.gameObject.CompareTag(GameConstants.UI_DUMMY_BLOCK_TAG))
                    {
                        if (result.gameObject.GetComponent<UIRaycastTargetable>() == null)
                        {
                            return false;
                        }
                        _container = result.gameObject.GetComponent<UIRaycastTargetable>().Root;
                        return true;
                    }
                }
            }

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == containerName)
                {
                    // BtnInput.Instance.ShowShadow(-1, Vector2.zero);
                    return _mode != BlockMode.BLOCK_ON_BLOCK;
                };
            }
            _container = null;
            return false;
        }
        public void OnBackButtonClick()
        {
            LevelManager.Instance.SetEndCauseLevelFinished(EndCause.Back_button);
            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_SELECTION));
        }
        public void OnHomeButtonClick()
        {
            LevelManager.Instance.SetEndCauseLevelFinished(EndCause.Back_button);
            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.HOME));
        }
        public void OnControlButtonActivate(int _type)
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke((ControlButton)_type, true);
        }
        public void OnControlButtonDeactivate(int _type)
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke((ControlButton)_type, false);
        }
        public void OnResetButtonClick()
        {
            ToolEvents.ON_RESET_BUTTON_CLICKED?.Invoke();
        }
        public void OnPreviewToggleButtonClick()
        {
            EditorEvents.ON_PREVIEW_STATUS_TOGGLE?.Invoke();
        }
        public void OnTogglePreview(bool _status)
        {
            EditorEvents.ON_FORCE_PREVIEW_STATUS_TOGGLE?.Invoke(_status);
        }
        public void CountBlocksUse()
        {
            foreach (Transform blockTransform in this.m_IDECodeContent)
            {
                UIBlock block = blockTransform.GetComponent<UIBlock>();
                if (block == null)
                    continue;
                switch (block.Type)
                {
                    case BlockType.MOVE_FORWARD:
                        LevelManager.Instance.SetStepForwardBlockCountUse();
                        break;
                    case BlockType.PICK_UP:
                        LevelManager.Instance.SetPickupBlockCountUse();
                        break;
                    case BlockType.PUSH:
                        LevelManager.Instance.SetPushBlockCountUse();
                        break;
                    case BlockType.PUT_DOWN:
                        LevelManager.Instance.SetPutDownBlockCountUse();
                        break;
                    case BlockType.TURN:
                        var blockTurn = (UIBlockTurn)block;
                        var blockOptionTurn = (UIBlockOptionTurn)blockTurn.UIBlockOption;
                        switch (blockOptionTurn.CurrentTurnDirection)
                        {
                            case TurnDirection.LEFT:
                                LevelManager.Instance.SetTurnLeftBlockCountUse();
                                break;
                            case TurnDirection.RIGHT:
                                LevelManager.Instance.SetTurnRightBlockCountUse();
                                break;
                            default:
                                LevelManager.Instance.SetTurnLeftBlockCountUse();
                                break;
                        }
                        break;
                    case BlockType.JUMP:
                        LevelManager.Instance.SetJumpBlockCountUse();
                        break;
                    case BlockType.JUMP_IF_STH_FRONT:
                        LevelManager.Instance.SetJumpIfBlockCountUse();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
