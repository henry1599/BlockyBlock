using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using UnityEngine.UI;
using BlockyBlock.Configurations;

namespace BlockyBlock.UI 
{   
    [System.Serializable]
    public class GridDirectionData
    {
        public ConditionDirection Type;
        public Image Image;
    }
    public class UIBlockOptionGrid : UIBlockOption
    {
        public GridDirectionData[] GridDirectionData;
        public Transform m_SnapPivot;
        public Color m_SeletedColor;
        public Color m_UnselectedColor;
        public bool IsOpenned
        {
            get => m_IsOpenned;
            set
            {
                m_IsOpenned = value;
                Selected(value);
            }
        } private bool m_IsOpenned;
        public ConditionDirection CurrentGridPosition
        {
            get => m_GridPosition;
            set 
            {
                m_GridPosition = value;
                UpdateOption(value);
            }
        } private ConditionDirection m_GridPosition;
        // public UIOptionTurn UIOptionTurn;

        void Start()
        {
            IsOpenned = false;
            CurrentGridPosition = ConditionDirection.TOP_MID;

            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED += HandleSelected;
            BlockEvents.ON_DISABLE_UI_FUNCTION += HandleDisableUIFunction;

            UIOptionGridDirection.ON_CLICK += HandleOptionItemClicked;
            // UIOptionItemSthFront.ON_OPTION_ITEM_CLICKED += HandleOptionItemClicked;
        }
        void OnDestroy() 
        {
            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED -= HandleSelected;
            BlockEvents.ON_DISABLE_UI_FUNCTION -= HandleDisableUIFunction;
            
            UIOptionGridDirection.ON_CLICK -= HandleOptionItemClicked;
            // UIOptionItemSthFront.ON_OPTION_ITEM_CLICKED -= HandleOptionItemClicked;
        }
        void HandleOptionItemClicked(ConditionDirection _gridDirection)
        {
            if (!IsOpenned)
            {
                return;
            }
            CurrentGridPosition = _gridDirection;
            HandleDisableUIFunction();
        }
        void HandleSelected(UIBlockOption _uiBlockOption, bool _status)
        {
            if (!(_uiBlockOption is UIBlockOptionGrid))
            {
                IsOpenned = false;
                return;
            }
            if ((UIBlockOptionGrid)_uiBlockOption != this)
            {
                return;
            }
            // * Show selection menu
            IsOpenned = _status;
        }
        void HandleDisableUIFunction()
        {
            Status = IsOpenned = false;
        }
        void Selected(bool _status)
        {
            UIManager.Instance.UIOptionGrid.Setup(_status, m_SnapPivot);
            // * Block ui ide
            if (_status)
            {
                BlockEvents.BLOCK_IDE_UI?.Invoke(UIBlock);
            }
            else
            {
                BlockEvents.UNBLOCK_IDE_UI?.Invoke(UIBlock);
            }
        }
        void UpdateOption(ConditionDirection _gridDirection)
        {
            // string result = "";
            // result = ConfigManager.Instance.GroundFrontDataConfig.Data[_groundType];
            // m_CurrentOptionText.text = result;
            foreach (GridDirectionData data in GridDirectionData)
            {
                if (data.Type == _gridDirection)
                {
                    data.Image.color = m_SeletedColor;
                }
                else
                {
                    data.Image.color = m_UnselectedColor;
                }
            }
        }
    }
}
