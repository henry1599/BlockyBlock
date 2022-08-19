using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Enums;
using UnityEngine.EventSystems;

namespace BlockyBlock.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance {get; private set;}
        public GraphicRaycaster m_Raycaster;
        public string m_IDECodeFieldName;
        public string m_ProviderCodeFieldName;
        public EventSystem m_EventSystem;
        PointerEventData m_PointerEventData;
        void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public bool CheckTriggerUI(BlockMode _mode)
        {
            string containerName = m_IDECodeFieldName;
            switch (_mode)
            {
                case BlockMode.IDE:
                    containerName = m_IDECodeFieldName;
                    break;
                case BlockMode.PREVIEW:
                    containerName = m_ProviderCodeFieldName;
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

            return false;


        }
    }
}
