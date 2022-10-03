using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core
{
    public class BlockFunctionJumpIfGrabSth : BlockFunction
    {
        public int IdxJumpTo = 0;
        public BlockFunctionJumpIfGrabSth(UIBlockJumpIfGrabSth _uiBlock) : base(_uiBlock, BlockType.JUMP_GRAB_STH)
        {
        }
        public void Setup()
        {
            IdxJumpTo = ((UIBlockJumpIfGrabSth)uiBlock).m_UIBlockJumpTo.transform.GetSiblingIndex();
        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_JUMP_IF_GRAB_STH?.Invoke(this);
        }
    }
}
