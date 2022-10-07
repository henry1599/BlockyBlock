using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Configurations;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.UI 
{
    public class UIBlockOptionSthFront : UIBlockOption
    {
        public GroundType[] Options;
        public Transform m_SnapPivot;
        public bool IsOpenned
        {
            get => m_IsOpenned;
            set
            {
                m_IsOpenned = value;
                Selected(value);
            }
        } private bool m_IsOpenned;
        public GroundType CurrentGroundType
        {
            get => m_GroundType;
            set 
            {
                m_GroundType = value;
                UpdateOption(value);
            }
        } private GroundType m_GroundType;
        // public UIOptionTurn UIOptionTurn;
        void Start()
        {
            IsOpenned = false;
            CurrentGroundType = GroundType.GROUND;

            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED += HandleSelected;
            BlockEvents.ON_DISABLE_UI_FUNCTION += HandleDisableUIFunction;

            UIOptionItemSthFront.ON_OPTION_ITEM_CLICKED += HandleOptionItemClicked;
        }
        void OnDestroy() 
        {
            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED -= HandleSelected;
            BlockEvents.ON_DISABLE_UI_FUNCTION -= HandleDisableUIFunction;
            
            UIOptionItemSthFront.ON_OPTION_ITEM_CLICKED -= HandleOptionItemClicked;
        }
        void HandleOptionItemClicked(int _idx)
        {
            if (!IsOpenned)
            {
                return;
            }
            CurrentGroundType = (GroundType)_idx;
            HandleDisableUIFunction();
        }
        void HandleSelected(UIBlockOption _uiBlockOption, bool _status)
        {
            if (!(_uiBlockOption is UIBlockOptionSthFront))
            {
                IsOpenned = false;
                return;
            }
            if ((UIBlockOptionSthFront)_uiBlockOption != this)
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
            UIManager.Instance.UIOptionSthFront.Setup(_status, m_SnapPivot, GetOptionStrings());
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
        List<string> GetOptionStrings()
        {
            List<string> result = new List<string>();
            List<GroundType> remainingOptions = new List<GroundType>();
            foreach (GroundType groundType in Options)
            {
                if (groundType != CurrentGroundType)
                {
                    remainingOptions.Add(groundType);
                }
            }
            foreach (GroundType groundType in remainingOptions)
            {
                result.Add(ConfigManager.Instance.GroundFrontDataConfig.Data[groundType]);
            }
            return result;
        }
        void UpdateOption(GroundType _groundType)
        {
            string result = "";
            result = ConfigManager.Instance.GroundFrontDataConfig.Data[_groundType];
            m_CurrentOptionText.text = result;
        }
    }
}
