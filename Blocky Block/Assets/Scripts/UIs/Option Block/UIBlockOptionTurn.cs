using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;

namespace BlockyBlock.UI 
{
    public class UIBlockOptionTurn : UIBlockOption
    {
        void Start()
        {
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
