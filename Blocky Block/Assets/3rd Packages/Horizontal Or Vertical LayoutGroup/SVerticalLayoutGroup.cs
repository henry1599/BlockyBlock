using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Events;

namespace Utility.SLayout
{
    [AddComponentMenu("Layout/SVertical Layout Group", 151)]
    public class SVerticalLayoutGroup : SHorizontalOrVerticalLayoutGroup
    {
        protected override void Start()
        {
            base.Start();
            BlockEvents.ON_UPDATE_VERTICAL_LAYOUT += HandleBlockHighlight;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            BlockEvents.ON_UPDATE_VERTICAL_LAYOUT += HandleBlockHighlight;
        }
        void HandleBlockHighlight()
        {
            SetChildrenAlongAxis(1, true);
        }
        protected SVerticalLayoutGroup()
        {}

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, true);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void CalculateLayoutInputVertical()
        {
            CalcAlongAxis(1, true);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0, true);
        }

        /// <summary>
        /// Called by the layout system. Also see ILayoutElement
        /// </summary>
        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1, true);
            int childCount = transform.childCount - 1;
            GameEvents.ON_TOGGLE_CONTROLLER_PANEL?.Invoke(childCount > 0);
            foreach (Transform child in transform)
            {
                if (child.GetComponent<UIBlock>() == null)
                {
                    continue;
                }
                child.GetComponent<UIBlock>().UpdateLineNumber();
            }
            EditorEvents.ON_IDE_CONTENT_CHANGED?.Invoke(transform.childCount);
        }
    }
}