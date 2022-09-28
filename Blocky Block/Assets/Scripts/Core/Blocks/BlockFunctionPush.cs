using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core 
{
    public class BlockFunctionPush : BlockFunction
    {
        public BlockFunctionPush(UIBlockPush _uiBlock) : base(_uiBlock, BlockType.PUSH)
        {

        }
        public void Setup()
        {

        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_PUSH?.Invoke(this);
        }
    }
}