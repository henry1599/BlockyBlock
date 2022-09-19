using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core
{
    public class BlockFunctionTurnLeft : BlockFunction
    {
        public BlockFunctionTurnLeft(UIBlock _uiBlock) : base(_uiBlock, BlockType.TURN)
        {

        }
        public void Setup()
        {
            
        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_TURN_LEFT?.Invoke(this);
        }
    }
}
