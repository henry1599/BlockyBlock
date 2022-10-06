using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core 
{
    public class BlockFunctionJumpIfSthFront : BlockFunction
    {
        public int IdxJumpTo = 0;
        public GroundType GroundFront;
        public BlockFunctionJumpIfSthFront(UIBlockJumpIfSthFront _uiBlock) : base(_uiBlock, BlockType.JUMP_IF_STH_FRONT)
        {
            GroundFront = _uiBlock.UIBlockOptionSthFront.CurrentGroundType;
        }
        public void Setup()
        {
            IdxJumpTo = ((UIBlockJumpIfSthFront)uiBlock).m_UIBlockJumpTo.transform.GetSiblingIndex();
        }
        public override void Execute()
        {
            // * Call Unit movement here
            UnitEvents.ON_JUMP_IF_STH_FRONT?.Invoke(this);
        }
    }
}
