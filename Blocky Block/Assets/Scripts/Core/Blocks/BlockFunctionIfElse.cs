using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core
{
    public class BlockFunctionIfElse : BlockFunction
    {
        public int IdxJumpTo = 0;
        public BlockFunctionIfElse(UIBlockIfElse _uiBlock) : base(_uiBlock, BlockType.IF_ELSE)
        {
        }
        public void Setup()
        {
            IdxJumpTo = ((UIBlockIfElse)uiBlock).m_UIBlockJumpTo.transform.GetSiblingIndex();
        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_JUMP_IF_ELSE?.Invoke(this);
        }
    }
}
