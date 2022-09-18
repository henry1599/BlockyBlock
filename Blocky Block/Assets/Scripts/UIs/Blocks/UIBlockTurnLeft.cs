using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace BlockyBlock.UI
{
    public class UIBlockTurnLeft : UIBlock
    {
        [SerializeField] UIBlockOptionTurn m_UIBlockOptionTurn;
        public override void Setup(UIBlock _parentBlock = null)
        {
            base.Setup(_parentBlock);
            m_UIBlockOptionTurn.Setup();
            // print("UIBlock Turn Left Setup");
        }
    }
}
