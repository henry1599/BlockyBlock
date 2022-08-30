using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Enums;

namespace BlockyBlock.Core
{
    public class BlockFunction
    {
        protected BlockType blockType;
        protected UIBlock uiBlock;
        public UIBlock UIBlock => uiBlock;
        public BlockType BlockType => blockType;
        public BlockFunction(UIBlock _uiBlock, BlockType _type)
        {
            this.uiBlock = _uiBlock;
            this.blockType = _type;
        }
        public virtual void AddSelfToQueue(ref List<BlockFunction> _queue){}
        public virtual void Execute(){}
        public virtual void Highlight()
        {
            uiBlock.HighlightSelf();
        }
    }
}
