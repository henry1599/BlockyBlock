using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;

namespace BlockyBlock.Core
{
    public class BlockFunction
    {
        protected UIBlock uiBlock;
        public UIBlock UIBlock => uiBlock;
        public BlockFunction(UIBlock _uiBlock)
        {
            this.uiBlock = _uiBlock;
        }
        public virtual void AddSelfToQueue(ref List<BlockFunction> _queue){}
        public virtual void Execute(){}
        public virtual void Highlight()
        {
            uiBlock.HighlightSelf();
        }
    }
}
