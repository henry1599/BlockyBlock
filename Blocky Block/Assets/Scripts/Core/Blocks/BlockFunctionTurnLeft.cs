using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;

namespace BlockyBlock.Core
{
    public class BlockFunctionTurnLeft : BlockFunction
    {
        public BlockFunctionTurnLeft(UIBlock _uiBlock) : base(_uiBlock)
        {

        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_TURN_LEFT?.Invoke();
        }
        public override void Highlight()
        {
            base.Highlight();
        }
    }
}
