using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;

namespace BlockyBlock.Core
{
    public class BlockFunctionMoveForward : BlockFunction
    {
        public BlockFunctionMoveForward(UIBlock _uiBlock) : base(_uiBlock)
        {

        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_MOVE_FORWARD?.Invoke();
        }
        public override void Highlight()
        {
            base.Highlight();
        }
    }
}
