using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core
{
    public class BlockFunctionJump : BlockFunction
    {
        public int IdxJumpTo = 0;
        public BlockFunctionJump(UIBlockJump _uiBlock) : base(_uiBlock, BlockType.JUMP)
        {
        }
        public void Setup()
        {
            IdxJumpTo = ((UIBlockJump)uiBlock).m_UIBlockJumpTo.transform.GetSiblingIndex();
        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_JUMP?.Invoke(this);
        }
    }
}
