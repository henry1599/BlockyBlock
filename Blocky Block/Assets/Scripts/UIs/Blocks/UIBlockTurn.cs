using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace BlockyBlock.UI
{
    public class UIBlockTurn : UIBlock
    {
        public override void Setup(UIBlock _parentBlock = null)
        {
            base.Setup(_parentBlock);
            ((UIBlockOptionTurn)UIBlockOption).Setup();
            // print("UIBlock Turn Left Setup");
        }
    }
}
