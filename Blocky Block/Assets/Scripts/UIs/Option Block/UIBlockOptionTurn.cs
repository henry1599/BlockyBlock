using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Configurations;

namespace BlockyBlock.UI 
{
    public class UIBlockOptionTurn : UIBlockOption
    {
        public TurnDirection[] Options;
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
        public TurnDirection CurrentTurnDirection
        {
            get => m_TurnDirection;
            set 
            {
                m_TurnDirection = value;
                UpdateOption(value);
            }
        } private TurnDirection m_TurnDirection;
        // public UIOptionTurn UIOptionTurn;
        void Start()
        {
            IsOpenned = false;
            CurrentTurnDirection = TurnDirection.LEFT;

            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED += HandleSelected;
            BlockEvents.ON_DISABLE_UI_FUNCTION += HandleDisableUIFunction;

            UIOptionItem.ON_OPTION_ITEM_CLICKED += HandleOptionItemClicked;
        }
        void OnDestroy() 
        {
            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED -= HandleSelected;
            BlockEvents.ON_DISABLE_UI_FUNCTION -= HandleDisableUIFunction;
            
            UIOptionItem.ON_OPTION_ITEM_CLICKED -= HandleOptionItemClicked;
        }
        void HandleOptionItemClicked(int _idx)
        {
            if (!IsOpenned)
            {
                return;
            }
            CurrentTurnDirection = (TurnDirection)_idx;
            HandleDisableUIFunction();
        }
        void HandleSelected(UIBlockOption _uiBlockOption, bool _status)
        {
            if ((UIBlockOptionTurn)_uiBlockOption != this)
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
            UIManager.Instance.UIOptionTurn.Setup(_status, m_SnapPivot, GetOptionStrings());
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
            List<TurnDirection> remainingOptions = new List<TurnDirection>();
            foreach (TurnDirection direction in Options)
            {
                if (direction != CurrentTurnDirection)
                {
                    remainingOptions.Add(direction);
                }
            }
            foreach (TurnDirection direction in remainingOptions)
            {
                result.Add(ConfigManager.Instance.TurnDirectionConfig.Data[direction]);
            }
            return result;
        }
        void UpdateOption(TurnDirection _turnDirection)
        {
            string result = "";
            result = ConfigManager.Instance.TurnDirectionConfig.Data[_turnDirection];
            m_CurrentOptionText.text = result;
        }
    }
}
