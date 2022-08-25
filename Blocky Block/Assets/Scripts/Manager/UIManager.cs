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

namespace BlockyBlock.Managers
{
    [System.Serializable]
    public class BlockData : SerializableDictionaryBase<BlockType, UIBlock> {}
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance {get; private set;}
        public GraphicRaycaster m_Raycaster;
        public GameObject m_IDECodeField;
        public Transform m_IDECodeContent;
        public GameObject m_PreviewCodeField;
        public Transform m_PreviewCodeContent;
        public EventSystem m_EventSystem;
        public BlockData m_BlockDatas;
        public Transform m_DummyUIBlock;
        PointerEventData m_PointerEventData;
        void Awake()
        {
            Instance = this;

            GameEvents.SETUP_LEVEL += HandleSetupLevel;
        }
        // Start is called before the first frame update
        void Start()
        {
            GameEvents.ON_CLEAR_IDE -= HandleClearIDE;
        }
        void OnDestroy()
        {
            GameEvents.SETUP_LEVEL -= HandleSetupLevel;
            GameEvents.ON_CLEAR_IDE -= HandleClearIDE;
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
        }
        void HandleSetupLevel(LevelData _data)
        {
            foreach (BlockType t in _data.BlockTypes)
            {
                GameObject uiBlockObject = Instantiate(m_BlockDatas[t].gameObject);
                uiBlockObject.transform.SetParent(m_PreviewCodeContent);
            }
            
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
                };
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
        public void OnHomeButtonClick()
        {
            GameEvents.LOAD_LEVEL?.Invoke(LevelID.HOME);
        }
        public void OnControlButtonActivate(int _type)
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke((ControlButton)_type, true);
        }
        public void OnControlButtonDeactivate(int _type)
        {
            GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke((ControlButton)_type, false);
        }
    }
}