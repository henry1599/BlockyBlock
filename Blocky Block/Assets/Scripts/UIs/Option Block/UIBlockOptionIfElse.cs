using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Configurations;

namespace BlockyBlock.UI 
{
    public class UIBlockOptionIfElse : UIBlockOption
    {
        public ConditionDirection[] Options;
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
        public ConditionDirection CurrentTurnDirection
        {
            get => m_TurnDirection;
            set 
            {
                m_TurnDirection = value;
                UpdateOption(value);
            }
        } private ConditionDirection m_TurnDirection;
        // Start is called before the first frame update
        void Start()
        {
            IsOpenned = false;
            CurrentTurnDirection = ConditionDirection.CENTER | ConditionDirection.MID;

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
            CurrentTurnDirection = (ConditionDirection)_idx;
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
            List<ConditionDirection> remainingOptions = new List<ConditionDirection>();
            foreach (ConditionDirection direction in Options)
            {
                if (direction != CurrentTurnDirection)
                {
                    remainingOptions.Add(direction);
                }
            }
            foreach (ConditionDirection direction in remainingOptions)
            {
                // result.Add(ConfigManager.Instance.TurnDirectionConfig.Data[direction]);
            }
            return result;
        }
        void UpdateOption(ConditionDirection _conditionData)
        {
            string result = "";
            // result = ConfigManager.Instance.TurnDirectionConfig.Data[_turnDirection];
            m_CurrentOptionText.text = result;
        }
    }
}
