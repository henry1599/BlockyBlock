using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.UI 
{
    public class UIBlockOptionTurn : UIBlockOption
    {
        public TurnDirection TurnDirection
        {
            get => m_TurnDirection;
            set 
            {
                m_TurnDirection = value;
            }
        } private TurnDirection m_TurnDirection;
        // public UIOptionTurn UIOptionTurn;
        void Start()
        {
            TurnDirection = TurnDirection.RIGHT;
            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED += HandleSelected;
        }
        void OnDestroy() 
        {
            BlockEvents.ON_UI_BLOCK_OPTION_SELECTED -= HandleSelected;
        }
        void HandleSelected(UIBlockOption _uiBlockOption, bool _status)
        {
            if ((UIBlockOptionTurn)_uiBlockOption != this)
            {
                return;
            }
            // * Show selection menu
            // UIOptionTurn.Setup(_status);
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
    }
}
