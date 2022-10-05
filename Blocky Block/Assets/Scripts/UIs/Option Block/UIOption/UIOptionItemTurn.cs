using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.UI 
{
    public class UIOptionItemTurn : UIOptionItem
    {
        public static System.Action<int> ON_OPTION_ITEM_CLICKED;
        public override void OnClick()
        {
            base.OnClick();
            ON_OPTION_ITEM_CLICKED?.Invoke(m_Idx);
        }
    }
}
