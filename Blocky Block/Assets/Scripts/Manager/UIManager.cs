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
        PointerEventData m_PointerEventData;
        void Awake()
        {
            Instance = this;

            GameEvents.SETUP_LEVEL += HandleSetupLevel;
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        void OnDestroy()
        {
            GameEvents.SETUP_LEVEL -= HandleSetupLevel;
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
                if (result.gameObject.name.Contains("Block"))
                {
                    _container = null;
                    return false;
                };
            }

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == containerName)
                {
                    // BtnInput.Instance.ShowShadow(-1, Vector2.zero);
                    return true;
                };
            }
            _container = null;
            return false;


        }
    }
}
