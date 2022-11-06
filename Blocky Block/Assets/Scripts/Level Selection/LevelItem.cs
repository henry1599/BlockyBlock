using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Utils;
using BlockyBlock.Events;
using DG.Tweening;
using EPOOutline;

namespace BlockyBlock
{

    public class LevelItem : MonoBehaviour
    {
        private int m_LevelID;
        bool m_IsHighlighted = false;
        void Start()
        {
            LevelSelectionEvents.ON_HIGHLIGHT_ITEM += HandleHighlight;
        }
        void OnDestroy()
        {
            LevelSelectionEvents.ON_HIGHLIGHT_ITEM -= HandleHighlight;
        }
        void HandleHighlight(LevelItem _item)
        {
            if (_item != this)
            {
                Unhighlight();
            }
            else
            {
                Highlight();
            }
        }
        void OnMouseDown()
        {
            if (!m_IsHighlighted)
            {
                LevelSelectionEvents.ON_HIGHLIGHT_ITEM?.Invoke(this);
                return;
            }
            OnClick();
        }
        void OnMouseEnter()
        {
            LevelSelectionEvents.ON_ITEM_HOVER?.Invoke(true);
        }
        void OnMouseExit()
        {
            LevelSelectionEvents.ON_ITEM_HOVER?.Invoke(false);
        }
        void Highlight()
        {
        }
        void Unhighlight()
        {
        }
        public void Setup(LevelID _levelId)
        {
            m_LevelID = (int)_levelId;
        }
        public void OnClick()
        {
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    GameEvents.LOAD_LEVEL?.Invoke((LevelID)m_LevelID);
                }
            );
        }
    }
}
