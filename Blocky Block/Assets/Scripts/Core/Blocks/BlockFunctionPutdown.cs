using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core 
{
    public class BlockFunctionPutdown : BlockFunction
    {
        public BlockFunctionPutdown(UIBlockPutdown _uiBlock) : base(_uiBlock, BlockType.PICK_UP)
        {

        }
        public void Setup()
        {

        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_PUT_DOWN?.Invoke(this);
        }
    }
}
